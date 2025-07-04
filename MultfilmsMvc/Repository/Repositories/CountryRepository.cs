using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Country>> GetCountriesWithCartoonsAsync()
        {
            return await _context.Countries
                 .Include(c => c.CountryCartoons)
                .Where(s => s.CountryCartoons.Any())
                .ToListAsync();
        }
        public async Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName)
        {
            return await _context.Countries
                .AnyAsync(c => c.Name == name && c.Image == imageFileName);
        }
    }
}

