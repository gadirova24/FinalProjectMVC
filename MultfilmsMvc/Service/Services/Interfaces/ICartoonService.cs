using System;
using Domain.Entities;
using Service.ViewModels.Admin.Cartoon;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ICartoonService
	{
        Task<AdminCartoonVM> GetByIdAsync(int id);
        Task<IEnumerable<CartoonVM>> GetAllAsync();
        Task<IEnumerable<AdminCartoonVM>> GetAllAdminAsync();
        Task CreateAsync(CartoonCreateVM cartoon);
        Task<CartoonDetailVM> GetDetailAsync(int id);
        Task DeleteAsync(int id);
        Task<Cartoon> GetByIdWithIncludesAsync(int id);
        Task<bool> IsDuplicateAsync(string name, string imageFileName);
        Task UpdateAsync(int id, CartoonEditVM cartoon);
        Task<IEnumerable<CartoonVM>> GetLatestCartoonsAsync(int take = 12);
        Task<AdminCartoonDetailVM> GetDetailByIdAsync(int id);
        Task<CartoonListVM> GetPaginatedCartoonsByCountryIdAsync(int countryId, int pageNumber, int pageSize = 12, string sortBy = "alphabet");
        Task<CartoonListVM> GetPaginatedCartoonsByGenreIdAsync(int genreId, int pageNumber, int pageSize = 12, string sortBy = "alphabet");
        Task<CartoonListVM> GetPaginatedCartoonsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize = 12, string sortBy = "alphabet");
        Task<IEnumerable<CartoonVM>> SearchByNameAsync(string name);
        Task<CartoonListVM> GetPaginatedCartoonsByStudioIdAsync(int studioId, int pageNumber, int pageSize = 12, string sortBy = "alphabet");
        Task<IEnumerable<CartoonDetailVM>> GetFavoritesByUserIdAsync(string userId);
        Task<IEnumerable<CartoonVM>> GetTopRatedCartoonsAsync(int count = 10);
    }
}

