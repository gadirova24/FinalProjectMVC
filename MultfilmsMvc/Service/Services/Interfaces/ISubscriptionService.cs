using System;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ISubscriptionService
	{
        Task<SubscriptionVM> CreateSubscriptionSessionAsync(string userId);
        Task<bool> ConfirmSubscriptionAsync(string sessionId);
        Task<bool> CheckSubscriptionStatusAsync(string userId);
    }
}

