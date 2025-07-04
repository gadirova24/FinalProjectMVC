using System;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Studio;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface IStudioService
	{
        Task<AdminStudioVM> GetByIdAsync(int id);
        Task<IEnumerable<StudioVM>> GetAllAsync();
        Task<IEnumerable<AdminStudioVM>> GetAllAdminAsync();
        Task CreateAsync(StudioCreateVM studio);
        Task DeleteAsync(int id);
        Task<bool> IsDuplicateAsync(string name, string imageFileName);
        Task UpdateAsync(int id, StudioEditVM studio);
        Task<IEnumerable<StudioVM>> GetStudiosWithCartoonsAsync();
        Task<StudioDetailVM> GetStudioDetailAsync(int id);
    }
}

