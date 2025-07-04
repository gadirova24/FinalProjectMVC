using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service.Services;
    using Service.Services.Interfaces;
    using Service.ViewModels.Admin.CartoonSlider;
    using Service.ViewModels.Admin.Collection;

    namespace MultfilmsMvc.Areas.Admin.Controllers
    {
        [Area("Admin")]
        public class CartoonSliderController : Controller
        {
            private readonly ICartoonSliderService _cartoonSliderService;

            public CartoonSliderController(ICartoonSliderService cartoonSliderService)
            {
                _cartoonSliderService = cartoonSliderService;
            }
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var sliders = await _cartoonSliderService.GetAllAdminAsync();
                return View(sliders);
            }
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CartoonSliderCreateVM model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                try
                {
                    await _cartoonSliderService.CreateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
            }
            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var slider = await _cartoonSliderService.GetByIdAsync(id);
                if (slider == null)
                    return NotFound();

                var editVM = new CartoonSliderEditVM
                {
                    CartoonId = id, 
                    ExistingImage = slider.BackgroundImage
                };

                return View(editVM);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, CartoonSliderEditVM request)
            {
                try
                {
                    await _cartoonSliderService.UpdateAsync(id, request);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to update slider.");
                    var slider = await _cartoonSliderService.GetByIdAsync(id);
                    request.ExistingImage = slider.BackgroundImage;
                    return View(request);
                }
                return RedirectToAction(nameof(Index));
            }

            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                try
                {
                    await _cartoonSliderService.DeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

}

