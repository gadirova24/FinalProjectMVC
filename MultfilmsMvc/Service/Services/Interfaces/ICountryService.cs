using System;
using Service.ViewModels.Admin.Country;

using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ICountryService
	{
        Task<AdminCountryVM> GetByIdAsync(int id);
        Task<IEnumerable<CountryVM>> GetAllAsync();
        Task<IEnumerable<AdminCountryVM>> GetAllAdminAsync();
        Task CreateAsync(CountryCreateVM country);
        Task DeleteAsync(int id);
        Task<bool> IsDuplicateAsync(string name);
        Task UpdateAsync(int id, CountryEditVM country);
        Task<IEnumerable<CountryVM>> GetCountriesWithCartoonsAsync();
        Task<CountryDetailVM> GetCountryDetailAsync(int id);
    }
}

