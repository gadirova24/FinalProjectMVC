using System;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity

    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
        public IQueryable<T> Query() => _entities.AsQueryable();

        public async Task<int> GetCountAsync()
        {
            return await _entities.CountAsync();
        }
    }
}

