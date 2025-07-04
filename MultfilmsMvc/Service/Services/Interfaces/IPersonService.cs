using System;
using Service.ViewModels.Admin.Genre;
using Service.ViewModels.Admin.Person;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface IPersonService
	{
        Task<AdminPersonVM> GetByIdAsync(int id);
        Task<IEnumerable<PersonVM>> GetAllAsync();
        Task<IEnumerable<AdminPersonVM>> GetAllAdminAsync();
        Task CreateAsync(PersonCreateVM genre);
        Task DeleteAsync(int id);
        Task<IEnumerable<AdminPersonVM>> GetDirectorsAsync();
        Task<PersonDetailVM> GetDetailAsync(int id);
        Task<IEnumerable<AdminPersonVM>> GetActorsAsync();
        Task<PersonEditVM> GetEditModelAsync(int id);
        Task<bool> UpdateAsync(int id, PersonEditVM model);
    }

}

