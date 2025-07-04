using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICountryService _countryService;
        private readonly ICollectionService _collectionService;
        private readonly IStudioService _studioService;
        private readonly ICartoonService _cartoonService;
        private readonly ICartoonSliderService _cartoonSliderService;
        public HomeController(ICountryService countryService,
                             ICollectionService collectionService,
                             IStudioService studioService,
                             ICartoonService cartoonService,
                             ICartoonSliderService cartoonSliderService)
        {
            _countryService = countryService;
            _collectionService = collectionService;
            _studioService = studioService;
            _cartoonSliderService = cartoonSliderService;
            _cartoonService = cartoonService;
        }
        public async Task< IActionResult> Index()
        {
         
            IEnumerable<CartoonVM> cartoons = await _cartoonService.GetLatestCartoonsAsync();
            IEnumerable<CartoonSliderVM> cartoonsLsiders = await _cartoonSliderService.GetAllAsync();
            IEnumerable<CartoonVM> topRatedCartoons = await _cartoonService.GetTopRatedCartoonsAsync();
            IEnumerable<StudioVM> studiosWithCartoons = await _studioService.GetStudiosWithCartoonsAsync();
            IEnumerable<CountryVM> countriesWithCartoons = await _countryService.GetCountriesWithCartoonsAsync();
            IEnumerable<CollectionVM> collectionsWithCartoons = await _collectionService.GetCollectionsWithCartoonsAsync();
            return View(new HomeVM
            {
                Countries = countriesWithCartoons,
                Collections=collectionsWithCartoons,
                Studios= studiosWithCartoons,
                Cartoons=cartoons,
                TopRated = topRatedCartoons,
                CartoonSliders=cartoonsLsiders



            }) ;
        }
    }
}

