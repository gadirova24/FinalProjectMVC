using System;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository
{
	public static class DependencyInjection
	{
        public static void AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IStudioRepository, StudioRepository>();
            services.AddScoped<ICartoonRepository, CartoonRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ICartoonSliderRepository, CartonSliderRepository>();
        }
    }
}

