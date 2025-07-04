using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Cartoon;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CartoonController : Controller
    {
        private readonly ICartoonService _cartoonService;
        private readonly IStudioService _studioService;
        private readonly IGenreService _genreService;
        private readonly ICountryService _countryService;
        private readonly ICollectionService _collectionService;
        private readonly ICategoryService _categoryService;
        private readonly IPersonService _personService;
        public CartoonController(ICartoonService cartoonService,
                                 IStudioService studioService,
                                 IGenreService genreService,
                                 ICountryService countryService,
                                 ICollectionService collectionService,
                                 ICategoryService categoryService,
                                 IPersonService personService)
        {
            _cartoonService = cartoonService;
            _studioService = studioService;
            _genreService = genreService;
            _countryService = countryService;
            _collectionService = collectionService;
            _categoryService = categoryService;
            _personService = personService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _cartoonService.GetAllAdminAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var detail = await _cartoonService.GetDetailByIdAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new CartoonCreateVM
            {
                CategoryOptions = (await _categoryService.GetAllAdminAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),

                DirectorOptions = (await _personService.GetDirectorsAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList(),

                ActorOptions = (await _personService.GetActorsAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList(),

                GenreOptions = (await _genreService.GetAllAdminAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),

                StudioOptions = (await _studioService.GetAllAdminAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),

                CollectionOptions = (await _collectionService.GetAllAdminAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),

                CountryOptions = (await _countryService.GetAllAdminAsync())
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList()
            };

            return View(vm);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartoonCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(request);
                return View(request);
            }

            var allCartoons = await _cartoonService.GetAllAdminAsync();
            var isDuplicate = allCartoons.Any(c =>
                c.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (isDuplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A cartoon with this name already exists.");
                await PopulateSelectListsAsync(request);
                return View(request);
            }

            await _cartoonService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var cartoonDetail = await _cartoonService.GetByIdWithIncludesAsync(id.Value);
            if (cartoonDetail == null) return NotFound();
            var vm = new CartoonEditVM
            {
                Name = cartoonDetail.Name,
                ExistImage = cartoonDetail.Image,
                Plot = cartoonDetail.Plot,
                Year = cartoonDetail.Year,
                PlayerUrl = cartoonDetail.PlayerUrl,
                SelectedCategoryId = cartoonDetail.CategoryId,
                SelectedDirectorIds = cartoonDetail.Directors.Select(d => d.PersonId).ToList(),
                SelectedActorIds = cartoonDetail.Actors.Select(a => a.PersonId).ToList(),
                SelectedGenreIds = cartoonDetail.CartoonGenres.Select(g => g.GenreId).ToList(),
                SelectedStudioIds = cartoonDetail.CartoonStudios.Select(s => s.StudioId).ToList(),
                SelectedCollectionIds = cartoonDetail.CartoonCollections.Select(c => c.CollectionId).ToList(),
                SelectedCountryIds = cartoonDetail.CountryCartoons.Select(c => c.CountryId).ToList()
            };

            await PopulateSelectListsAsync(vm);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CartoonEditVM request)
        {
            var allCartoons = await _cartoonService.GetAllAdminAsync();
            var isDuplicate = allCartoons.Any(c =>
                c.Id != id && c.Name.Trim().ToLower() == request.Name.Trim().ToLower());

            if (isDuplicate)
            {
                ModelState.AddModelError(nameof(request.Name), "A cartoon with this name already exists.");
                await PopulateSelectListsAsync(request);
                return View(request);
            }

            try
            {
                await _cartoonService.UpdateAsync(id, request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await PopulateSelectListsAsync(request);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartoonService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private async Task PopulateSelectListsAsync(CartoonEditVM vm)
        {
            vm.CategoryOptions = (await _categoryService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.DirectorOptions = (await _personService.GetDirectorsAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            vm.ActorOptions = (await _personService.GetActorsAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            vm.GenreOptions = (await _genreService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.StudioOptions = (await _studioService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.CollectionOptions = (await _collectionService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.CountryOptions = (await _countryService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        private async Task PopulateSelectListsAsync(CartoonCreateVM vm)
        {
            vm.CategoryOptions = (await _categoryService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.DirectorOptions = (await _personService.GetDirectorsAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            vm.ActorOptions = (await _personService.GetActorsAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            vm.GenreOptions = (await _genreService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.StudioOptions = (await _studioService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.CollectionOptions = (await _collectionService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.CountryOptions = (await _countryService.GetAllAdminAsync())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

    }
}

