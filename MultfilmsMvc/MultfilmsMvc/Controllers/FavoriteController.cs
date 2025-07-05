using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ICartoonService _cartoonService;
        private readonly ISubscriptionService _subService;
        private readonly UserManager<AppUser> _userManager;

        public FavoriteController(IFavoriteService favoriteService,
                                 UserManager<AppUser> userManager,
                                 ICartoonService cartoonService,
                                 ISubscriptionService subService)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
            _cartoonService = cartoonService;
            _subService = subService;
        }

      
        [HttpPost]
        public async Task<IActionResult> Toggle([FromForm] int cartoonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
            }

            var isFavorite = await _favoriteService.IsFavoriteAsync(cartoonId, userId);

            if (isFavorite)
                await _favoriteService.RemoveFromFavoritesAsync(cartoonId, userId);
            else
                await _favoriteService.AddToFavoritesAsync(cartoonId, userId);

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Purchase", "Subscription");

            var hasSubscription = await _subService.HasActiveSubscriptionAsync(userId);
            if (!hasSubscription)
                return RedirectToAction("Purchase", "Subscription");

            var favorites = await _cartoonService.GetFavoritesByUserIdAsync(userId);
            return View(favorites);
        }
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = _userManager.GetUserId(User);
            await _favoriteService.ClearFavoritesAsync(userId);
            return RedirectToAction("Index");
        }
    }

}

