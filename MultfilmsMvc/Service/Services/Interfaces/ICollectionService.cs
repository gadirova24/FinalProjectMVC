using System;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ICollectionService
    {
       
        Task<AdminCollectionVM> GetByIdAsync(int id);
        Task<IEnumerable<CollectionVM>> GetAllAsync();
        Task<IEnumerable<AdminCollectionVM>> GetAllAdminAsync();
        Task CreateAsync(CollectionCreateVM collection);
        Task DeleteAsync(int id);
        Task<bool> IsDuplicateAsync(string name, string imageFileName);
        Task UpdateAsync(int id, CollectionEditVM collection);
        Task<CollectionDetailVM> GetCollectionDetailAsync(int id);
        Task<IEnumerable<CollectionVM>> GetCollectionsWithCartoonsAsync();

    }
}

