using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Studio;



namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class StudioController : Controller
    {
        private readonly IStudioService _studioService;
        public StudioController(IStudioService studioService)
        {
            _studioService = studioService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _studioService.GetAllAdminAsync());
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

            var detail = await _studioService.GetStudioDetailAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudioCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var allStudios = await _studioService.GetAllAdminAsync();
            var duplicate = allStudios.Any(s =>
                s.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (duplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A studio with this name already exists.");
                return View(request);
            }

            await _studioService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _studioService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var studio = await _studioService.GetByIdAsync(id.Value);
            if (studio is null) return NotFound();
            return View(new StudioEditVM
            {
                ExistImage = studio.Image,
                Name = studio.Name

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudioEditVM request)
        {
            var allStudios = await _studioService.GetAllAdminAsync();
            var duplicate = allStudios.Any(s =>
                s.Id != id &&
                s.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (duplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A studio with this name already exists.");
                var existing = await _studioService.GetByIdAsync(id);
                request.ExistImage = existing.Image;
                return View(request);
            }

            try
            {
                await _studioService.UpdateAsync(id, request);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to update studio.");
                var studio = await _studioService.GetByIdAsync(id);
                request.ExistImage = studio.Image;
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}

