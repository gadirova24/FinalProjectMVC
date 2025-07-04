using System;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers.Enums;
using Service.ViewModels.Admin.Cartoon;
using Service.ViewModels.Admin.CartoonSlider;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Genre;
using Service.ViewModels.Admin.Person;
using Service.ViewModels.Admin.Studio;
using Service.ViewModels.UI;
using static Service.ViewModels.Admin.CartoonSlider.AdminCartoonSliderVM;

namespace Service.Helpers.Mappings
{
	public class MappingProfile:Profile
	{
		public MappingProfile()
		{
            //country
            CreateMap<Country, AdminCountryVM>();
            CreateMap<Country, CountryVM>()
            .ForMember(dest => dest.CartoonCount,opt => opt.MapFrom(src => src.CountryCartoons.Count));
            CreateMap<CountryCreateVM, Country>();
            CreateMap<CountryEditVM, Country>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Country, CountryDetailVM>();

            //studio

            CreateMap<Studio, StudioVM>().ForMember(dest => dest.CartoonCount,opt => opt.MapFrom(src => src.CartoonStudios.Count));
            CreateMap<Studio, AdminStudioVM>();
            CreateMap<StudioCreateVM, Studio>();
            CreateMap<Studio, StudioDetailVM>();
            CreateMap<StudioEditVM, Studio>();
            //collection
            CreateMap<Collection, AdminCollectionVM>();
            CreateMap<Collection, CollectionVM>().ForMember(dest => dest.CartoonCount, opt => opt.MapFrom(src => src.CartoonCollections.Count));
            CreateMap<CollectionCreateVM, Collection>();
            CreateMap<CollectionEditVM, Collection>();
            CreateMap<Collection, CollectionDetailVM>();

            //genre
            CreateMap<Genre, GenreVM>();
            CreateMap<Genre, AdminGenreVM>();
            CreateMap<GenreCreateVM, Genre>();
            CreateMap<GenreEditVM, Genre>();
            CreateMap<Genre, GenreDetailVM>();

            //category
            CreateMap<Category, CategoryVM>();
            CreateMap<Category, AdminCategoryVM>();
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<CategoryEditVM, Category>();
            CreateMap<CategoryDetailVM, Category>();



            //cartoons
            CreateMap<Cartoon, CartoonVM>()
           .ForMember(dest => dest.AverageRating, opt =>opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Value) : 0))
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
     
            CreateMap<Cartoon, AdminCartoonDetailVM>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Cartoon, AdminCartoonVM>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CartoonCreateVM, Cartoon>()
            .ForMember(dest => dest.PlayerUrl, opt => opt.MapFrom(src => src.PlayerUrl))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId)) 
            .ForMember(dest => dest.Directors, opt =>opt.MapFrom(src => src.SelectedDirectorIds.Select(id => new CartoonDirector { PersonId = id })))
            .ForMember(dest => dest.Actors, opt =>opt.MapFrom(src => src.SelectedActorIds.Select(id => new CartoonActor { PersonId = id })))
            .ForMember(dest => dest.CartoonGenres, opt => opt.MapFrom(src => src.SelectedGenreIds.Select(id => new CartoonGenre { GenreId = id })))
            .ForMember(dest => dest.CartoonStudios, opt =>  opt.MapFrom(src => src.SelectedStudioIds.Select(id => new CartoonStudio { StudioId = id })))
            .ForMember(dest => dest.CartoonCollections, opt =>opt.MapFrom(src => src.SelectedCollectionIds.Select(id => new CartoonCollection { CollectionId = id })))
            .ForMember(dest => dest.CountryCartoons, opt => opt.MapFrom(src => src.SelectedCountryIds.Select(id => new CountryCartoon { CountryId = id })));

            CreateMap<CartoonEditVM, Cartoon>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()) 
            .ForMember(dest => dest.Directors, opt =>  opt.MapFrom(src => src.SelectedDirectorIds.Select(id => new CartoonDirector { PersonId = id })))
            .ForMember(dest => dest.Actors, opt =>opt.MapFrom(src => src.SelectedActorIds.Select(id => new CartoonActor { PersonId = id })))
            .ForMember(dest => dest.CartoonGenres, opt =>opt.MapFrom(src => src.SelectedGenreIds.Select(id => new CartoonGenre { GenreId = id })))
            .ForMember(dest => dest.CartoonStudios, opt =>opt.MapFrom(src => src.SelectedStudioIds.Select(id => new CartoonStudio { StudioId = id })))
            .ForMember(dest => dest.CartoonCollections, opt => opt.MapFrom(src => src.SelectedCollectionIds.Select(id => new CartoonCollection { CollectionId = id })))
            .ForMember(dest => dest.CountryCartoons, opt =>  opt.MapFrom(src => src.SelectedCountryIds.Select(id => new CountryCartoon { CountryId = id })));

            CreateMap<Cartoon, CartoonDetailVM>()
       .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
       .ForMember(dest => dest.CountryNames, opt => opt.MapFrom(src => src.CountryCartoons.Select(cc => cc.Country.Name)))
       .ForMember(dest => dest.StudioNames, opt => opt.MapFrom(src => src.CartoonStudios.Select(cs => cs.Studio.Name)))
       .ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src => src.CartoonGenres.Select(cg => cg.Genre.Name)))
       .ForMember(dest => dest.DirectorNames, opt => opt.MapFrom(src => src.Directors.Select(d => d.Person.FullName)))
       .ForMember(dest => dest.ActorNames, opt => opt.MapFrom(src => src.Actors.Select(a => a.Person.FullName)));


            //person


            CreateMap<Person, PersonVM>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.RoleType.ToString())));
            CreateMap<Person, AdminPersonVM>()
            .ForMember(dest => dest.Roles, opt =>opt.MapFrom(src => src.Roles.Select(r => r.RoleType.ToString())));
            CreateMap<PersonCreateVM, Person>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
            CreateMap<Person, PersonDetailVM>();



            CreateMap<CartoonSlider, CartoonSliderVM>()
            .ForMember(dest => dest.CartoonId, opt => opt.MapFrom(src => src.CartoonId))
            .ForMember(dest => dest.CartoonName, opt => opt.MapFrom(src => src.Cartoon.Name))
            .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Cartoon.Plot))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Cartoon.Ratings.Any() ? src.Cartoon.Ratings.Average(r => r.Value) : 0))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Cartoon.Year))
            .ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src => src.Cartoon.CartoonGenres.Select(cg => cg.Genre.Name).ToList()))
            .ForMember(dest => dest.BackgroundImage, opt => opt.MapFrom(src => src.BackgroundImage));

            CreateMap<CartoonSlider, AdminCartoonSliderVM>()
            .ForMember(dest => dest.CartoonName, opt => opt.MapFrom(src => src.Cartoon.Name));
            CreateMap<CartoonSliderCreateVM, CartoonSlider>();
            CreateMap<CartoonSlider, CartoonSliderEditVM>()
            .ForMember(dest => dest.ExistingImage, opt => opt.MapFrom(src => src.BackgroundImage));
        }
    }
}

