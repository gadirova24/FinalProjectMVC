using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<bool> AnyAsync(Expression<Func<Favorite, bool>> predicate);
        Task<Favorite> GetAsync(Expression<Func<Favorite, bool>> predicate);
        Task AddAsync(Favorite favorite);
        void Remove(Favorite favorite);
        IQueryable<Favorite> Query();
        Task SaveChangesAsync();
    }
}

