
using System;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Person;
using Service.ViewModels.UI;

namespace Service.Services
{
    public class PersonService : IPersonService

    {
        private readonly IPersonRepository _personRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public PersonService(IPersonRepository personRepo,
                             IMapper mapper,
                             AppDbContext context)
        {
            _personRepo = personRepo;
            _mapper = mapper;
            _context = context;

        }
        public async Task CreateAsync(PersonCreateVM model)
        {
            var person = new Person
            {
                FullName = model.FullName,
                Roles = new List<PersonRole>()
            };

            if (model.SelectedRole.HasValue)
            {
                person.Roles.Add(new PersonRole
                {
                    RoleType = model.SelectedRole.Value
                });
            }
            await _personRepo.CreateAsync(person);
        }
        public async Task DeleteAsync(int id)
        {
            var data = await _personRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            await _personRepo.DeleteAsync(data);
        }
        public async Task<PersonDetailVM> GetDetailAsync(int id)
        {
            var person = await _personRepo.Query()
                .Include(p => p.CartoonActors)
                    .ThenInclude(ca => ca.Cartoon)
                .Include(p => p.CartoonDirectors)
                    .ThenInclude(cd => cd.Cartoon)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return null;

            var cartoonNames = person.CartoonActors
                .Select(ca => ca.Cartoon.Name)
                .Union(person.CartoonDirectors.Select(cd => cd.Cartoon.Name))
                .Distinct()
                .ToList();

            var roles = new List<string>();
            if (person.CartoonActors.Any()) roles.Add("Actor");
            if (person.CartoonDirectors.Any()) roles.Add("Director");

            return new PersonDetailVM
            {
                FullName = person.FullName,
                Roles = roles,
                CartoonCount = cartoonNames.Count,
                CartoonNames = cartoonNames
            };
        }
        public async Task<IEnumerable<AdminPersonVM>> GetAllAdminAsync()
        {
            var people = await _personRepo.Query().Include(p => p.Roles).ToListAsync();
            return _mapper.Map<IEnumerable<AdminPersonVM>>(people);
        }

        public async Task<IEnumerable<PersonVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<PersonVM>>(await _personRepo.GetAllAsync());
        }
        public async Task<PersonEditVM> GetEditModelAsync(int id)
        {
            var person = await _context.Persons
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) throw new Exception("Person not found");

            return new PersonEditVM
            {
                FullName = person.FullName,
                SelectedRoles = person.Roles.Select(r => r.RoleType).ToList(),
                RoleOptions = Enum.GetValues(typeof(PersonRoleType))
                    .Cast<PersonRoleType>()
                    .Select(r => new SelectListItem
                    {
                        Text = r.ToString(),
                        Value = ((int)r).ToString(),
                        Selected = person.Roles.Any(pr => pr.RoleType == r)
                    }).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, PersonEditVM model)
        {
            var person = await _context.Persons
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) throw new Exception("Person not found");

            var fullNameChanged = person.FullName != model.FullName.Trim();
            var roleChanged = !person.Roles.Select(r => r.RoleType).OrderBy(x => x)
                .SequenceEqual(model.SelectedRoles.OrderBy(x => x));

            if (!fullNameChanged && !roleChanged)
                return false;

            person.FullName = model.FullName.Trim();
            person.Roles.Clear();
            foreach (var role in model.SelectedRoles)
            {
                person.Roles.Add(new PersonRole { RoleType = role, PersonId = id });
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AdminPersonVM> GetByIdAsync(int id)
        {
            var data = await _personRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminPersonVM>(data);
        }
        public async Task<IEnumerable<AdminPersonVM>> GetDirectorsAsync()
        {
            var persons = await _personRepo.Query()
        .Include(p => p.Roles) 
        .Where(p => p.Roles.Any(r => r.RoleType == PersonRoleType.Director))
        .ToListAsync();

            var vmList = persons.Select(p => new AdminPersonVM
            {
                Id = p.Id,
                FullName = p.FullName,
                Roles = p.Roles.Select(r => r.RoleType.ToString()).ToList()
            });
            return vmList;
        }
        public async Task<IEnumerable<AdminPersonVM>> GetActorsAsync()
        {
            var persons = await _personRepo.Query()
                .Include(p => p.Roles)
                .Where(p => p.Roles.Any(r => r.RoleType == PersonRoleType.Actor))
                .ToListAsync();

            return persons.Select(p => new AdminPersonVM
            {
                Id = p.Id,
                FullName = p.FullName,
                Roles = p.Roles.Select(r => r.RoleType.ToString()).ToList()
            });
        }

    }
}

