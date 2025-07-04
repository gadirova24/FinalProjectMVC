using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetGenresWithCartoonsAsync();
    }
}

