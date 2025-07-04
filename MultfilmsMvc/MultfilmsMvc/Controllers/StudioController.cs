using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

namespace MultfilmsMvc.Controllers
{
    public class StudioController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly IStudioService _studioService;

        public StudioController(ICartoonService cartoonService,
                                IStudioService studioService)
        {
            _cartoonService = cartoonService;
            _studioService = studioService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id, int page = 1, string sort = "alphabet")
        {
            var studio = await _studioService.GetByIdAsync(id);
            if (studio == null) return NotFound();

            var cartoonList = await _cartoonService.GetPaginatedCartoonsByStudioIdAsync(id, page, 12, sort);

            ViewBag.StudioName = studio.Name;
            ViewBag.StudioId = id;
            ViewBag.Sort = sort;
            ViewBag.HasCartoons = cartoonList.Cartoons.Any();
            //ViewBag.Studios = await _studioService.GetAllAsync();

            return View(cartoonList);
        }

    }
}

