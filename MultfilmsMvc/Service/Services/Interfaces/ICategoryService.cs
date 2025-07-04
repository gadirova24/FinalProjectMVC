using System;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ICategoryService
	{
        Task<AdminCategoryVM> GetByIdAsync(int id);
        Task<IEnumerable<CategoryVM>> GetAllAsync();
        Task<IEnumerable<AdminCategoryVM>> GetAllAdminAsync();
        Task CreateAsync(CategoryCreateVM category);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, CategoryEditVM category);
        Task<CategoryDetailVM> GetDetailByIdAsync(int id);
        
    }
}

