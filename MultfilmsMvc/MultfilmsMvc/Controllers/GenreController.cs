using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    public class GenreController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly IGenreService _genreService;
        public GenreController(ICartoonService cartoonService,
                                    IGenreService genreService)
        {
            _cartoonService = cartoonService;
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id, int page = 1, string sort = "alphabet")
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();

            var cartoonList = await _cartoonService.GetPaginatedCartoonsByGenreIdAsync(id, page, 12, sort);

            ViewBag.GenreName = genre.Name;
            ViewBag.GenreId = id;
            ViewBag.Sort = sort;
            ViewBag.HasCartoons = cartoonList.Cartoons != null && cartoonList.Cartoons.Any();
            ViewBag.Genres = await _genreService.GetAllAsync();
            return View(cartoonList);
        }
    }
}

