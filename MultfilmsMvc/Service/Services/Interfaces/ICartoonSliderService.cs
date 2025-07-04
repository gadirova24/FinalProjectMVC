using System;
using Domain.Entities;
using Service.ViewModels.Admin.CartoonSlider;
using Service.ViewModels.UI;
using static Service.ViewModels.Admin.CartoonSlider.AdminCartoonSliderVM;

namespace Service.Services.Interfaces
{
	public interface ICartoonSliderService
	{
        Task<IEnumerable<CartoonSliderVM>> GetAllAsync();
        Task<IEnumerable<AdminCartoonSliderVM>> GetAllAdminAsync();
        Task<CartoonSliderVM> GetByIdAsync(int id);
        Task CreateAsync(CartoonSliderCreateVM model);
        Task UpdateAsync(int id,CartoonSliderEditVM model);
        Task DeleteAsync(int id);
    }
}

