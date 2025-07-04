using System;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

namespace MultfilmsMvc.ViewComponents
{
   
        public class SidebarViewComponent : ViewComponent
        {
            private readonly IStudioService _studioService;
            private readonly ICollectionService _collectionService;
            private readonly IGenreService _genreService;
            private readonly ICountryService _countryService;

            public SidebarViewComponent(
                IStudioService studioService,
                ICollectionService collectionService,
                IGenreService genreService,
                ICountryService countryService)
            {
                _studioService = studioService;
                _collectionService = collectionService;
                _genreService = genreService;
                _countryService = countryService;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                var studios = await _studioService.GetAllAsync();
                var collections = await _collectionService.GetAllAsync();
                var genres = await _genreService.GetAllAsync();
                var countries = await _countryService.GetAllAsync();

                var model = new SidebarVM
                {
                    Studios = studios,
                    Collections = collections,
                    Genres = genres,
                    Countries = countries
                };

                return View(model);
            }
        }
    }



