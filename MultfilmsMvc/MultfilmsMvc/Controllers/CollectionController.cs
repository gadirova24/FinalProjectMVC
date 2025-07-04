using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly ICollectionService _collectionService;
        public CollectionController(ICartoonService cartoonService,
                                    ICollectionService collectionService)
        {
            _cartoonService = cartoonService;
            _collectionService = collectionService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id, int page = 1, string sort = "alphabet")
        {
            var collection = await _collectionService.GetByIdAsync(id);
            if (collection == null) return NotFound();

            var cartoonList = await _cartoonService.GetPaginatedCartoonsByCollectionIdAsync(id, page, 12, sort);

            ViewBag.CollectionName = collection.Name;
            ViewBag.Sort = sort;
            ViewBag.CollectionId = id;
            ViewBag.HasCartoons = cartoonList.Cartoons != null && cartoonList.Cartoons.Any();
            ViewBag.Collections = await _collectionService.GetAllAsync();
            return View(cartoonList);
        }
    }
}

