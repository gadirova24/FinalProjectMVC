using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface IStudioRepository:IBaseRepository<Studio>
	{
        Task<IEnumerable<Studio>> GetStudiosWithCartoonsAsync();
        Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName);
    }
}

