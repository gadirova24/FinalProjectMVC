using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Helpers;
using Service.Services.Interfaces;
using Stripe.Checkout;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    [Route("[controller]/[action]")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subService;

        public SubscriptionController(ISubscriptionService subService)
        {
            _subService = subService;
        }

        [HttpGet]
        public IActionResult Purchase(int cartoonId)
        {
            ViewData["CartoonId"] = cartoonId;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(int cartoonId)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 2000,
                        Currency = "usd",
                        ProductData = new() { Name = "Single Cartoon Access" }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = $"{domain}/Subscription/Success?cartoonId={cartoonId}",
                CancelUrl = $"{domain}/Subscription/Cancel"
            };

            var session = new SessionService().Create(options);
            return Redirect(session.Url);
        }

        [HttpGet]
        public async Task<IActionResult> Success(int cartoonId)
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (uid != null)
            {
                await _subService.ActivateSubscriptionAsync(uid);
            }

            return RedirectToAction("Detail", "Cartoon", new { id = cartoonId });
        }

        [HttpGet]
        public IActionResult Cancel() => RedirectToAction("Purchase");
    }

}

