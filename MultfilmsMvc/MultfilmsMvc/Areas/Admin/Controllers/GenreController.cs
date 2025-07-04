using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Genre;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllAdminAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var detail = await _genreService.GetGenreDetailAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _genreService.CreateAsync(request);

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _genreService.GetByIdAsync(id.Value);
            if (category is null) return NotFound();

            return View(new GenreEditVM
            {
                Name = category.Name
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GenreEditVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _genreService.UpdateAsync(id, request);
            return RedirectToAction(nameof(Index));
        }

    }
}

