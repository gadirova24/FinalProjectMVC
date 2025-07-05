using System;
using Microsoft.AspNetCore.Http;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private const string SubscriptionSessionKey = "UserSubscriptions";

        public async Task ActivateSubscriptionAsync(string userId)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var subscriptionsJson = session.GetString(SubscriptionSessionKey);
            Dictionary<string, bool> subscriptions;

            if (string.IsNullOrEmpty(subscriptionsJson))
                subscriptions = new Dictionary<string, bool>();
            else
                subscriptions = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, bool>>(subscriptionsJson);

            subscriptions[userId] = true;

            session.SetString(SubscriptionSessionKey, System.Text.Json.JsonSerializer.Serialize(subscriptions));

            await Task.CompletedTask;
        }

        public Task<bool> HasActiveSubscriptionAsync(string userId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var subscriptionsJson = session.GetString(SubscriptionSessionKey);

            if (string.IsNullOrEmpty(subscriptionsJson))
                return Task.FromResult(false);

            var subscriptions = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, bool>>(subscriptionsJson);

            return Task.FromResult(subscriptions != null && subscriptions.ContainsKey(userId) && subscriptions[userId]);
        }
    }



}

