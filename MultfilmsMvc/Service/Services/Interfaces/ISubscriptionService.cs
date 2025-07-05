using System;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ISubscriptionService
	{
        Task ActivateSubscriptionAsync(string userId);
        Task<bool> HasActiveSubscriptionAsync(string userId);
    }
}

