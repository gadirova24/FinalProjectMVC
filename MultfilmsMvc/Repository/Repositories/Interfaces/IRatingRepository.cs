using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> GetAsync(Expression<Func<Rating, bool>> predicate);
        Task AddAsync(Rating rating);
        Task SaveChangesAsync();
        Task<double> GetAverageAsync(int cartoonId);
        IQueryable<Rating> Query();
    }
}

