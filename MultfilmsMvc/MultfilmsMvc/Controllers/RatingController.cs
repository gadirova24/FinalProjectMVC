using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly UserManager<AppUser> _userManager;

        public RatingController(IRatingService ratingService, UserManager<AppUser> userManager)
        {
            _ratingService = ratingService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Rate(int cartoonId, int value)
        {
            if (value < 1 || value > 5) return BadRequest();

            var userId = _userManager.GetUserId(User);
            await _ratingService.RateAsync(cartoonId, userId, value);

            var avg = await _ratingService.GetAverageRatingAsync(cartoonId);
            return Json(new { success = true, average = avg });
        }
    }

}

