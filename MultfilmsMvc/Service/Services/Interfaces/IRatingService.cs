using System;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
    public interface IRatingService
    {
        Task RateAsync(int cartoonId, string userId, int value);
        Task<double> GetAverageRatingAsync(int cartoonId);
        Task<List<int>> GetTopRatedCartoonIdsAsync(int count);
    }
}

