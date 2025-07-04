using System;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Genre;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface IGenreService
	{
        Task<AdminGenreVM> GetByIdAsync(int id);
        Task<IEnumerable<GenreVM>> GetAllAsync();
        Task<IEnumerable<AdminGenreVM>> GetAllAdminAsync();
        Task CreateAsync(GenreCreateVM genre);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, GenreEditVM genre);
        Task<GenreDetailVM> GetGenreDetailAsync(int id);
        Task<IEnumerable<GenreVM>> GetGenresWithCartoonsAsync();
    }
}

