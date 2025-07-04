using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface ICollectionRepository:IBaseRepository<Collection>
	{
        Task<IEnumerable<Collection>> GetCollectionsWithCartoonsAsync();
        Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName);
    }
}

