using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class StudioRepository:BaseRepository<Studio>,IStudioRepository
	{
        private readonly AppDbContext _context;
		public StudioRepository(AppDbContext context) : base(context)
        {
            _context = context;
		}
        public async Task<IEnumerable<Studio>> GetStudiosWithCartoonsAsync()
        {
            return await _context.Studios
                  .Include(c => c.CartoonStudios)
                .Where(s => s.CartoonStudios.Any()) 
                .ToListAsync();
        }
        public async Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName)
        {
            return await _context.Studios
                .AnyAsync(c => c.Name == name && c.Image == imageFileName);
        }
    }
}

