using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Genre;
using Service.ViewModels.UI;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepo,
                              IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task CreateAsync(CategoryCreateVM category)
        {
            await _categoryRepo.CreateAsync(_mapper.Map<Category>(category));
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _categoryRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            await _categoryRepo.DeleteAsync(data);
        }

        public async Task<IEnumerable<AdminCategoryVM>> GetAllAdminAsync()
        {
            return _mapper.Map<IEnumerable<AdminCategoryVM>>(await _categoryRepo.GetAllAsync());
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryVM>>(await _categoryRepo.GetAllAsync());
        }

        public async Task<AdminCategoryVM> GetByIdAsync(int id)
        {
            var data = await _categoryRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminCategoryVM>(data);
        }
   
        public async Task UpdateAsync(int id, CategoryEditVM category)
        {
            var existData = await _categoryRepo.GetByIdAsync(id);
            _mapper.Map(category, existData);
            await _categoryRepo.UpdateAsync(_mapper.Map<Category>(existData));
        }
        public async Task<CategoryDetailVM> GetDetailByIdAsync(int id)
        {
            var category = await _categoryRepo.Query().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return null;

            return new CategoryDetailVM
            {
                Name = category.Name
            };
        }
    }
}

