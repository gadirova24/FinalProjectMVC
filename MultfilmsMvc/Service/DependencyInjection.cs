using System;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Services.Interfaces;

namespace Service
{
	public static class DependencyInjection
	{
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddScoped<IStudioService, StudioService>();
            services.AddScoped<ICartoonService, CartoonService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICartoonSliderService, CartoonSliderService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            return services;

        }
    }
}

