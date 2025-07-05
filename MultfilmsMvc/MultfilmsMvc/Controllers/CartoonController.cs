using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
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
        private readonly ICommentService _commentService;
        private readonly IFavoriteService _favoriteService;
        private readonly ISubscriptionService _subService;
        public CartoonController(ICartoonService cartoonService,
                                IRatingService ratingService,
                                  UserManager<AppUser> userManager,
                                  IFavoriteService favoriteService,
                                  ICommentService commentService,
                                  ISubscriptionService subService)
        {
            _cartoonService = cartoonService;
            _ratingService = ratingService;
            _userManager = userManager;
            _favoriteService = favoriteService;
            _commentService = commentService;
            _subService = subService;
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return NotFound();  
            }
            var cartoon = await _cartoonService.GetDetailAsync(id);
            if (cartoon == null)
                return NotFound();

            if (!User.Identity.IsAuthenticated)
            {
               
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Detail", "Cartoon", new { id }) });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var hasSubscription = await _subService.HasActiveSubscriptionAsync(userId);
            if (!hasSubscription)
            {

                return RedirectToAction("Purchase", "Subscription", new { cartoonId = id });
            }


            cartoon.IsFavorite = await _favoriteService.IsFavoriteAsync(id, userId);
            cartoon.Comments = await _commentService.GetCommentsAsync(id);
            cartoon.NewComment = new CommentCreateVM { CartoonId = id };

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

