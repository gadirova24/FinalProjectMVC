using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CollectionRepository:BaseRepository<Collection>,ICollectionRepository
	{
        private readonly AppDbContext _context;
        public CollectionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Collection>> GetCollectionsWithCartoonsAsync()
        {
            return await _context.Collections
                 .Include(c => c.CartoonCollections)
                .Where(s => s.CartoonCollections.Any())
                .ToListAsync();
        }
        public async Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName)
        {
            return await _context.Collections
                .AnyAsync(c => c.Name == name && c.Image == imageFileName);
        }
    }
}

