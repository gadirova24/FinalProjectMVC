using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

namespace Service.Services
{

    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task RateAsync(int cartoonId, string userId, int value)
        {
            var existing = await _ratingRepository.GetAsync(r => r.CartoonId == cartoonId && r.UserId == userId);

            if (existing != null)
            {
                existing.Value = value;
            }
            else
            {
                await _ratingRepository.AddAsync(new Rating
                {
                    CartoonId = cartoonId,
                    UserId = userId,
                    Value = value
                });
            }

            await _ratingRepository.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingAsync(int cartoonId)
        {
            return await _ratingRepository.GetAverageAsync(cartoonId);
        }
        public async Task<List<int>> GetTopRatedCartoonIdsAsync(int count)
        {
            return await _ratingRepository.Query()
                .GroupBy(r => r.CartoonId)
                .Select(g => new
                {
                    CartoonId = g.Key,
                    AvgRating = g.Average(r => r.Value)
                })
                .OrderByDescending(g => g.AvgRating)
                .Take(count)
                .Select(g => g.CartoonId)
                .ToListAsync();
        }

    }


}

