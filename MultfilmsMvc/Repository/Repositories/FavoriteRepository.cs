using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<Favorite, bool>> predicate)
        {
            return await _context.Favorites.AnyAsync(predicate);
        }

        public async Task<Favorite> GetAsync(Expression<Func<Favorite, bool>> predicate)
        {
            return await _context.Favorites.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
        }

        public void Remove(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
        }
        public IQueryable<Favorite> Query()
        {
            return _context.Favorites.AsQueryable();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}

