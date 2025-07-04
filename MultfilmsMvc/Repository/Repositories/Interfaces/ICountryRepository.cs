using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<IEnumerable<Country>> GetCountriesWithCartoonsAsync();
        Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName);
    }
}

