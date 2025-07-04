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
    public class CountryController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly ICountryService _countryService;
        public CountryController(ICartoonService cartoonService,
                                 ICountryService countryService)
        {
            _cartoonService = cartoonService;
            _countryService = countryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id, int page = 1, string sort = "alphabet")
        {
            var country = await _countryService.GetByIdAsync(id);
            if (country == null) return NotFound();

            var cartoonList = await _cartoonService.GetPaginatedCartoonsByCountryIdAsync(id, page, 12, sort);

            ViewBag.CountryName = country.Name;
            ViewBag.Sort = sort;
            ViewBag.CountryId = id;
            ViewBag.HasCartoons = cartoonList.Cartoons != null && cartoonList.Cartoons.Any();

            return View(cartoonList);
        }
    }
}

