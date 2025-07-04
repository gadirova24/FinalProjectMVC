using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    public class CartoonController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly IRatingService _ratingService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFavoriteService _favoriteService;
        public CartoonController(ICartoonService cartoonService,
                                IRatingService ratingService,
                                  UserManager<AppUser> userManager,
                                  IFavoriteService favoriteService)
        {
            _cartoonService = cartoonService;
            _ratingService = ratingService;
            _userManager = userManager;
            _favoriteService = favoriteService;
        }

      

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var cartoon = await _cartoonService.GetDetailAsync(id);
            if (cartoon == null)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    cartoon.IsFavorite = await _favoriteService.IsFavoriteAsync(id, userId);
                }
            }

            var watchedJson = Request.Cookies["WatchedCartoons"];
            var watchedList = string.IsNullOrEmpty(watchedJson)
                ? new List<CartoonCookieVM>()
                : JsonSerializer.Deserialize<List<CartoonCookieVM>>(watchedJson);

            if (!watchedList.Any(c => c.Id == id))
            {
                watchedList.Add(new CartoonCookieVM
                {
                    Id = id,
                    Name = cartoon.Name,
                    Image = cartoon.Image,
                    CategoryName = cartoon.CategoryName,
                    Year = cartoon.Year
                });

                if (watchedList.Count > 10)
                    watchedList = watchedList.Skip(watchedList.Count - 10).ToList();

                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(30),
                    IsEssential = true
                };

                Response.Cookies.Append("WatchedCartoons", JsonSerializer.Serialize(watchedList), options);
            }

            return View(cartoon);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
         
            ViewBag.Query = query;

            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<CartoonVM>()); 
            }
            var results = await _cartoonService.SearchByNameAsync(query);
            return View(results.ToList());
        }
   

        [HttpGet]
        public IActionResult YouWatched()
        {
            var watchedJson = Request.Cookies["WatchedCartoons"];
            var watchedList = string.IsNullOrEmpty(watchedJson)
                ? new List<CartoonCookieVM>()
                : JsonSerializer.Deserialize<List<CartoonCookieVM>>(watchedJson);

            return View(watchedList);
        }
        public IActionResult ClearWatched()
        {
            Response.Cookies.Delete("WatchedCartoons");
            return RedirectToAction("YouWatched");
        }
    }

}

