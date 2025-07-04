using System;
using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetAsync(Expression<Func<Rating, bool>> predicate)
        {
            return await _context.Ratings.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageAsync(int cartoonId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.CartoonId == cartoonId)
                .Select(r => r.Value)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }
        public IQueryable<Rating> Query()
        {
            return _context.Ratings.AsQueryable();
        }
    }

}

