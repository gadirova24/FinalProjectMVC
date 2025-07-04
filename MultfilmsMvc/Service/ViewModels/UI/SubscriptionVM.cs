using System;
namespace Service.ViewModels.UI
{
	public class SubscriptionVM
	{
        public string UserId { get; set; }
        public string StripeSessionId { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime? SubscriptionExpiry { get; set; }
    }
}

