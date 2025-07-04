using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
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

            private readonly ICartoonService _cartoonService;
            public CartoonSliderController(ICartoonSliderService cartoonSliderService,
                                          ICartoonService cartoonService)
            {
                _cartoonSliderService = cartoonSliderService;
                _cartoonService = cartoonService;
            }
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var sliders = await _cartoonSliderService.GetAllAdminAsync();
                return View(sliders);
            }
      

            [HttpGet]
            public async Task<IActionResult> Create()
            {
                var cartoons = await _cartoonService.GetAllAdminAsync();
                var vm = new CartoonSliderCreateVM
                {
                    CartoonOptions = cartoons.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                return View(vm);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CartoonSliderCreateVM model)
            {
                if (!ModelState.IsValid)
                {
                    var cartoons = await _cartoonService.GetAllAdminAsync();
                    model.CartoonOptions = cartoons.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                    return View(model);
                }

                try
                {
                    await _cartoonSliderService.CreateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    var cartoons = await _cartoonService.GetAllAdminAsync();
                    model.CartoonOptions = cartoons.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                    return View(model);
                }
            }
            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var slider = await _cartoonSliderService.GetByIdAsync(id);
                if (slider == null) return NotFound();

                var cartoons = await _cartoonService.GetAllAdminAsync(); 
                var cartoonOptions = cartoons.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                var editVM = new CartoonSliderEditVM
                {
                    CartoonId = slider.CartoonId,
                    ExistingImage = slider.BackgroundImage,
                    CartoonOptions = cartoonOptions
                };

                return View(editVM);
            }


            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Edit(int id, CartoonSliderEditVM request)
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        var cartoons = await _cartoonService.GetAllAdminAsync();
            //        request.CartoonOptions = cartoons.Select(c => new SelectListItem
            //        {
            //            Value = c.Id.ToString(),
            //            Text = c.Name
            //        }).ToList();

            //        return View(request);
            //    }

            //    try
            //    {
            //        await _cartoonSliderService.UpdateAsync(id, request);
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", ex.Message);

            //        var cartoons = await _cartoonService.GetAllAdminAsync();
            //        request.CartoonOptions = cartoons.Select(c => new SelectListItem
            //        {
            //            Value = c.Id.ToString(),
            //            Text = c.Name
            //        }).ToList();

            //        return View(request);
            //    }

            //    return RedirectToAction(nameof(Index));
            //}


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, CartoonSliderEditVM request)
            {
                // Populate cartoon options again (needed if returning view)
                var cartoons = await _cartoonService.GetAllAdminAsync();
                request.CartoonOptions = cartoons.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                try
                {
                    // Call your service method that might throw exceptions on invalid input
                    await _cartoonSliderService.UpdateAsync(id, request);
                }
                catch (ValidationException vex)
                {
                    // You can pass error message to the view through ViewData or ViewBag
                    ViewData["Error"] = vex.Message;

                    // Return same view with errors and data
                    return View(request);
                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    ViewData["Error"] = "An error occurred while updating the slider.";

                    return View(request);
                }

                // Redirect on success
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

