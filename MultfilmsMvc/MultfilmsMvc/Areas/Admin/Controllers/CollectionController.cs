using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.Admin.Country;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;
        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;   
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _collectionService.GetAllAdminAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            var isDuplicate = await _collectionService.IsDuplicateAsync(request.Name, request.Image.FileName);
            if (isDuplicate)
            {
                ModelState.AddModelError("", "Collection with this name exist.");
                return View(request);
            }

            await _collectionService.CreateAsync(request);

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var detail = await _collectionService.GetCollectionDetailAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectionService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var collection = await _collectionService.GetByIdAsync(id);
            if (collection is null) return NotFound();
            return View(new CollectionEditVM
            {
                ExistImage = collection.Image,
                Name = collection.Name

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CollectionEditVM request)
        {
            try
            {
                await _collectionService.UpdateAsync(id, request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to update collection.");
                var collection = await _collectionService.GetByIdAsync(id);
                request.ExistImage = collection.Image;
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }
    }


}

