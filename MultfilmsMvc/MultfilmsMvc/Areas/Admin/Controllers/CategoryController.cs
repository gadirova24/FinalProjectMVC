using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Category;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllAdminAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var allCategories = await _categoryService.GetAllAdminAsync();
            var isDuplicate = allCategories.Any(c =>
                c.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (isDuplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A category with this name already exists.");
                return View(request);
            }

            await _categoryService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryService.GetByIdAsync(id.Value);
            if (category is null) return NotFound();

            return View(new CategoryEditVM
            {
                Name = category.Name
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var allCategories = await _categoryService.GetAllAdminAsync();
            var isDuplicate = allCategories.Any(c =>
                c.Id != id && c.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (isDuplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A category with this name already exists.");
                return View(request);
            }

            await _categoryService.UpdateAsync(id, request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var category = await _categoryService.GetDetailByIdAsync(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }
    }
}

