using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task AddToFavoritesAsync(int cartoonId, string userId)
        {
            if (!await _favoriteRepository.AnyAsync(f => f.UserId == userId && f.CartoonId == cartoonId))
            {
                await _favoriteRepository.AddAsync(new Favorite
                {
                    UserId = userId,
                    CartoonId = cartoonId
                });
                await _favoriteRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavoritesAsync(int cartoonId, string userId)
        {
            var favorite = await _favoriteRepository.GetAsync(f => f.UserId == userId && f.CartoonId == cartoonId);
            if (favorite != null)
            {
                _favoriteRepository.Remove(favorite);
                await _favoriteRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(int cartoonId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            return await _favoriteRepository.AnyAsync(f => f.UserId == userId && f.CartoonId == cartoonId);
        }
        public async Task ClearFavoritesAsync(string userId)
        {
            var favorites = await _favoriteRepository.Query()
                .Where(f => f.UserId == userId)
                .ToListAsync();

            if (favorites.Any())
            {
                foreach (var favorite in favorites)
                {
                    _favoriteRepository.Remove(favorite);
                }

                await _favoriteRepository.SaveChangesAsync();
            }
        }
    }

}

