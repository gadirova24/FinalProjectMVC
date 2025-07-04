using System;
using Domain.Entities;

namespace Service.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task AddToFavoritesAsync(int cartoonId, string userId);
        Task RemoveFromFavoritesAsync(int cartoonId, string userId);
        Task<bool> IsFavoriteAsync(int cartoonId, string userId);
        Task ClearFavoritesAsync(string userId);
    }
}

