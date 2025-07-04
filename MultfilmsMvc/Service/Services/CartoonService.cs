using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Cartoon;
using Service.ViewModels.UI;

namespace Service.Services
{
	public class CartoonService:ICartoonService
	{
        private readonly ICartoonRepository _cartoonRepo;
        private readonly IMapper _mapper;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IRatingService _ratingService;

        private readonly IWebHostEnvironment _env;
        public CartoonService(ICartoonRepository cartoonRepo,
                              IMapper mapper,
                              IWebHostEnvironment env,
                              IFavoriteRepository favoriteRepository,
                              IRatingService ratingService)
        {
            _cartoonRepo = cartoonRepo;
            _mapper = mapper;
            _env = env;
            _favoriteRepository = favoriteRepository;
            _ratingService = ratingService;
        }

        public async Task CreateAsync(CartoonCreateVM cartoon)
        {
              if (!cartoon.Image.CheckFileType("image/"))
                {
                    throw new ValidationException("Image type must be only image");
                }
                if (!cartoon.Image.CheckFileSize(500))
                {
                    throw new ValidationException("Image size must be smaller than 500KB");
                }

               string fileName = Guid.NewGuid().ToString() + " " + cartoon.Image.FileName;
                string filePath = _env.GenerateFilePath("assets/images/cartoons", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await cartoon.Image.CopyToAsync(stream);
                }
            

            var data = _mapper.Map<Cartoon>(cartoon);
            data.Image = fileName;

            await _cartoonRepo.CreateAsync(data);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _cartoonRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            if (!string.IsNullOrEmpty(data.Image))
            {
                string filePath = _env.GenerateFilePath("assets/images/cartoons", data.Image);
                filePath.DeleteFile();
            }
            await _cartoonRepo.DeleteAsync(data);
        }

        public async Task<IEnumerable<AdminCartoonVM>> GetAllAdminAsync()
        {
      
            return _mapper.Map<IEnumerable<AdminCartoonVM>>(await _cartoonRepo.GetAllAsync());
        }

        public async Task<IEnumerable<CartoonVM>> GetAllAsync()
        {
            var cartoons = await _cartoonRepo.Query()
                                           .Include(c => c.Category)
                                           .ToListAsync();
            return _mapper.Map<IEnumerable<CartoonVM>>(cartoons);
        }
        public async Task<bool> IsDuplicateAsync(string name, string imageFileName)
        {
            return await _cartoonRepo.ExistsByNameAndImageAsync(name, imageFileName);
        }
        public async Task<AdminCartoonVM> GetByIdAsync(int id)
        {
         var cartoon = await _cartoonRepo.Query()
        .Include(c => c.Category)
        .Include(c => c.CountryCartoons).ThenInclude(cc => cc.Country)
        .Include(c => c.CartoonStudios).ThenInclude(cs => cs.Studio)
        .Include(c => c.CartoonCollections).ThenInclude(cc => cc.Collection)
        .Include(c => c.CartoonGenres).ThenInclude(cc => cc.Genre)
        .Include(c => c.Directors).ThenInclude(cd => cd.Person)
        .Include(c => c.Actors).ThenInclude(ca => ca.Person)
        .Include(c => c.Ratings)
        .Include(c => c.Comments)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (cartoon == null)
                throw new NotFoundException("Cartoon not found.");

            return _mapper.Map<AdminCartoonVM>(cartoon);
        }
        public async Task<IEnumerable<CartoonVM>> GetLatestCartoonsAsync(int take = 12)
        {
            var cartoons = await _cartoonRepo.Query()
                .OrderByDescending(c => c.CreatedAt)  
                .Take(take)
                .Include(c => c.Category)
                .Include(c => c.Ratings)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartoonVM>>(cartoons);
        }
        public async Task UpdateAsync(int id, CartoonEditVM model)
        {
            var existingCartoon = await _cartoonRepo.GetByIdWithIncludesAsync(id);
            if (existingCartoon == null)
                throw new Exception("Cartoon not found");

            _mapper.Map(model, existingCartoon);

            if (model.UploadImage != null)
            {
                if (!string.IsNullOrWhiteSpace(existingCartoon.Image))
                {
                    var oldFilePath = _env.GenerateFilePath("assets/images/cartoons", existingCartoon.Image);
                    oldFilePath.DeleteFile();
                }

                var newFileName = Guid.NewGuid() + "-" + model.UploadImage.FileName;
                var newFilePath = _env.GenerateFilePath("assets/images/cartoons", newFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.UploadImage.CopyToAsync(stream);
                }

                existingCartoon.Image = newFileName;
            }
            existingCartoon.Directors.Clear();
            foreach (var directorId in model.SelectedDirectorIds)
            {
                existingCartoon.Directors.Add(new CartoonDirector { PersonId = directorId, CartoonId = id });
            }

            existingCartoon.Actors.Clear();
            foreach (var actorId in model.SelectedActorIds)
            {
                existingCartoon.Actors.Add(new CartoonActor { PersonId = actorId, CartoonId = id });
            }

            existingCartoon.CartoonGenres.Clear();
            foreach (var genreId in model.SelectedGenreIds)
            {
                existingCartoon.CartoonGenres.Add(new CartoonGenre { GenreId = genreId, CartoonId = id });
            }

            existingCartoon.CartoonStudios.Clear();
            foreach (var studioId in model.SelectedStudioIds)
            {
                existingCartoon.CartoonStudios.Add(new CartoonStudio { StudioId = studioId, CartoonId = id });
            }

            existingCartoon.CartoonCollections.Clear();
            foreach (var collectionId in model.SelectedCollectionIds)
            {
                existingCartoon.CartoonCollections.Add(new CartoonCollection { CollectionId = collectionId, CartoonId = id });
            }

            existingCartoon.CountryCartoons.Clear();
            foreach (var countryId in model.SelectedCountryIds)
            {
                existingCartoon.CountryCartoons.Add(new CountryCartoon { CountryId = countryId, CartoonId = id });
            }

            if (model.SelectedCategoryId.HasValue)
            {
                existingCartoon.CategoryId = model.SelectedCategoryId.Value;
            }
            else
            {
                throw new Exception("Category is required");
            }

            await _cartoonRepo.UpdateAsync(existingCartoon);
        }

        public async Task<IEnumerable<CartoonDetailVM>> GetFavoritesByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<CartoonDetailVM>();
            var favoriteCartoonIds = await _favoriteRepository.Query()
                .Where(f => f.UserId == userId)
                .Select(f => f.CartoonId)
                .ToListAsync();

            if (!favoriteCartoonIds.Any())
                return Enumerable.Empty<CartoonDetailVM>();
            var favoriteCartoons = await _cartoonRepo.Query()
                .Where(c => favoriteCartoonIds.Contains(c.Id))
                .Include(c => c.Category)
                 .Include(c => c.Ratings)
                .Include(c => c.CountryCartoons).ThenInclude(cc => cc.Country)
                .Include(c => c.CartoonStudios).ThenInclude(cs => cs.Studio)
                .Include(c => c.CartoonGenres).ThenInclude(cg => cg.Genre)
                .Include(c => c.Directors).ThenInclude(d => d.Person)
                .Include(c => c.Actors).ThenInclude(a => a.Person)
                .Include(c => c.Ratings)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<CartoonDetailVM>>(favoriteCartoons);

            foreach (var cartoon in result)
            {
                cartoon.IsFavorite = true;
                var cartoonEntity = favoriteCartoons.FirstOrDefault(c => c.Id == cartoon.Id);
                cartoon.AverageRating = cartoonEntity?.Ratings.Any() == true
                    ? cartoonEntity.Ratings.Average(r => r.Value)
                    : 0;
            }
           

            return result;
        }

        public async Task<AdminCartoonDetailVM> GetDetailByIdAsync(int id)
        {
                var cartoon = await _cartoonRepo.Query()
                    .Include(c => c.Category)
                    .Include(c => c.CartoonStudios).ThenInclude(cs => cs.Studio)
                    .Include(c => c.CartoonCollections).ThenInclude(cc => cc.Collection)
                    .Include(c => c.CountryCartoons).ThenInclude(cc => cc.Country)
                    .Include(c => c.CartoonGenres).ThenInclude(cg => cg.Genre)
                    .Include(c => c.Actors).ThenInclude(ca => ca.Person)
                    .Include(c => c.Directors).ThenInclude(cd => cd.Person)
                    .Include(c => c.Ratings)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cartoon == null) return null;

                return new AdminCartoonDetailVM
                {
                    Name = cartoon.Name,
                    Image = cartoon.Image,
                    Plot = cartoon.Plot,
                    PlayerUrl = cartoon.PlayerUrl,
                    Year = cartoon.Year,
                    CategoryName = cartoon.Category?.Name,

                    StudioNames = cartoon.CartoonStudios.Select(cs => cs.Studio.Name).ToList(),
                    CollectionNames = cartoon.CartoonCollections.Select(cc => cc.Collection.Name).ToList(),
                    CountryNames = cartoon.CountryCartoons.Select(cc => cc.Country.Name).ToList(),
                    GenreNames = cartoon.CartoonGenres.Select(g => g.Genre.Name).ToList(),

                    ActorNames = cartoon.Actors.Select(a => a.Person.FullName).ToList(),
                    DirectorNames = cartoon.Directors.Select(d => d.Person.FullName).ToList(),

                    AverageRating = cartoon.Ratings.Any() ? cartoon.Ratings.Average(r => r.Value) : 0
                };
            }

        public async Task<Cartoon> GetByIdWithIncludesAsync(int id)
        {
            return await _cartoonRepo.GetByIdWithIncludesAsync(id);
        }
        public async Task<IEnumerable<CartoonVM>> SearchByNameAsync(string name)
        {
            var cartoons = await _cartoonRepo.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<CartoonVM>>(cartoons);
        }
        public async Task<CartoonListVM> GetPaginatedCartoonsByStudioIdAsync(int studioId, int pageNumber, int pageSize = 12, string sortBy = "alphabet")
        {
            var (cartoons, totalCount) = await _cartoonRepo.GetPaginatedCartoonsByStudioIdAsync(studioId, pageNumber, pageSize,sortBy);

            var cartoonVMs = _mapper.Map<IEnumerable<CartoonVM>>(cartoons);

            return new CartoonListVM
            {
                Cartoons = cartoonVMs,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<CartoonListVM> GetPaginatedCartoonsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize = 12, string sortBy = "alphabet")
        {
            var (cartoons, totalCount) = await _cartoonRepo.GetPaginatedCartoonsByCollectionIdAsync(collectionId, pageNumber, pageSize, sortBy);

            var cartoonVMs = _mapper.Map<IEnumerable<CartoonVM>>(cartoons);

            return new CartoonListVM
            {
                Cartoons = cartoonVMs,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
        public async Task<CartoonDetailVM> GetDetailAsync(int id)
        {
            var cartoon = await _cartoonRepo.Query()
                .Include(c => c.Category)
                .Include(c => c.CartoonStudios).ThenInclude(cs => cs.Studio)
                .Include(c => c.CountryCartoons).ThenInclude(cc => cc.Country)
                .Include(c => c.CartoonCollections).ThenInclude(cc => cc.Collection)
                .Include(c => c.CartoonGenres).ThenInclude(cc => cc.Genre)
                .Include(c => c.Directors).ThenInclude(cd => cd.Person)
                .Include(c => c.Actors).ThenInclude(ca => ca.Person)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartoon == null) return null;
            var data = _mapper.Map<CartoonDetailVM>(cartoon);
            data.AverageRating = await _ratingService.GetAverageRatingAsync(cartoon.Id); 

            return _mapper.Map<CartoonDetailVM>(data);
        }
        public async Task<IEnumerable<CartoonVM>> GetTopRatedCartoonsAsync(int count = 10)
        {
            var topRatedIds = await _ratingService.GetTopRatedCartoonIdsAsync(count);
            var cartoons = await _cartoonRepo.Query()
                .Include(c => c.Category)
                   .Include(c => c.Ratings)
                .Include(c => c.CountryCartoons).ThenInclude(cc => cc.Country)
                .Include(c => c.CartoonStudios).ThenInclude(cs => cs.Studio)
                .Include(c => c.CartoonCollections).ThenInclude(cc => cc.Collection)
                .Include(c => c.CartoonGenres).ThenInclude(cg => cg.Genre)
                .Include(c => c.Directors).ThenInclude(d => d.Person)
                .Include(c => c.Actors).ThenInclude(a => a.Person)
                .Where(c => topRatedIds.Contains(c.Id))
                .ToListAsync();
                 var ordered = topRatedIds
                .Select(id => cartoons.FirstOrDefault(c => c.Id == id))
                .Where(c => c != null);

            return _mapper.Map<IEnumerable<CartoonVM>>(ordered);
        }

        public async Task<CartoonListVM> GetPaginatedCartoonsByCountryIdAsync(int countryId, int pageNumber, int pageSize = 12, string sortBy = "alphabet")
        {
            var (cartoons, totalCount) = await _cartoonRepo.GetPaginatedCartoonsByCountryIdAsync(countryId, pageNumber, pageSize, sortBy);

            var cartoonVMs = _mapper.Map<IEnumerable<CartoonVM>>(cartoons);

            return new CartoonListVM
            {
                Cartoons = cartoonVMs,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
        public async Task<CartoonListVM> GetPaginatedCartoonsByGenreIdAsync(int genreId, int pageNumber, int pageSize = 12, string sortBy = "alphabet")
        {
            var (cartoons, totalCount) = await _cartoonRepo.GetPaginatedCartoonsByGenreIdAsync(genreId, pageNumber, pageSize, sortBy);

            var cartoonVMs = _mapper.Map<IEnumerable<CartoonVM>>(cartoons);

            return new CartoonListVM
            {
                Cartoons = cartoonVMs,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}

