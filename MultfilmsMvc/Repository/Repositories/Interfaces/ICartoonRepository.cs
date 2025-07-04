using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface ICartoonRepository:IBaseRepository<Cartoon>
	{
      
        Task<IEnumerable<Cartoon>> SearchByNameAsync(string name);
        Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize, string sortBy = "alphabet");
        Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByCountryIdAsync(int countryId, int pageNumber, int pageSize, string sortBy = "alphabet");
        Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByGenreIdAsync(int genreId, int pageNumber, int pageSize, string sortBy = "alphabet");
        Task<Cartoon> GetByIdWithIncludesAsync(int id);
        Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByStudioIdAsync(int studioId, int pageNumber, int pageSize, string sortBy = "alphabet");
        Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName);
    }
}

