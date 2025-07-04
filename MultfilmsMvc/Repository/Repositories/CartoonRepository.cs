using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CartoonRepository:BaseRepository<Cartoon>,ICartoonRepository
	{

        private readonly AppDbContext _context;

        public CartoonRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cartoon>> SearchByNameAsync(string name)
        {
            return await _context.Cartoons
                                 .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                                 .Include(c => c.Category)
                                 .Include(c => c.CartoonGenres)
                                 .Include(c => c.CartoonStudios)
                                 .Include(c => c.CountryCartoons)
                                 .Include(c => c.CartoonCollections)
                                 .Include(c => c.Directors)
                                 .Include(c => c.Actors)
                                 .Include(c => c.Ratings)
                                 .ToListAsync();
        }

            public async Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByStudioIdAsync(int studioId, int pageNumber, int pageSize, string sortBy = "alphabet")
        {
            var query = _context.Cartoons
                .Include(c => c.Category)
                .Include(c => c.Ratings)
                .Include(c => c.CartoonStudios)
                .ThenInclude(cs => cs.Studio)
                .Where(c => c.CartoonStudios.Any(cs => cs.StudioId == studioId));

            switch (sortBy?.ToLower())
            {
                case "date":
                    query = query.OrderByDescending(c => c.CreatedAt); 
                    break;

                case "alphabet":
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            int totalCount = await query.CountAsync();

            var cartoons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cartoons, totalCount);
        }
        public async Task<Cartoon> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Cartoons
                .Include(c => c.Directors)
                    .ThenInclude(cd => cd.Person)
                .Include(c => c.Actors)
                    .ThenInclude(ca => ca.Person)
                .Include(c => c.CartoonGenres)
                    .ThenInclude(cg => cg.Genre)
                .Include(c => c.CartoonStudios)
                    .ThenInclude(cs => cs.Studio)
                .Include(c => c.CartoonCollections)
                    .ThenInclude(cc => cc.Collection)
                .Include(c => c.CountryCartoons)
                    .ThenInclude(cc => cc.Country)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsByNameAndImageAsync(string name, string imageFileName)
        {
            return await _context.Cartoons
                .AnyAsync(c => c.Name == name && c.Image == imageFileName);
        }
        public async Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize, string sortBy = "alphabet")
        {
            var query = _context.Cartoons
                .Include(c => c.Category)
                .Include(c => c.Ratings)
                .Include(c => c.CartoonCollections)
                .ThenInclude(cs => cs.Collection)
                .Where(c => c.CartoonCollections.Any(cs => cs.CollectionId == collectionId));

            switch (sortBy?.ToLower())
            {
                case "date":
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;

                case "alphabet":
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            int totalCount = await query.CountAsync();

            var cartoons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cartoons, totalCount);
        }


        public async Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByCountryIdAsync(int countryId, int pageNumber, int pageSize, string sortBy = "alphabet")
        {
            var query = _context.Cartoons
                .Include(c => c.Category)
                .Include(c => c.Ratings)
                .Include(c => c.CountryCartoons)
                .ThenInclude(cs => cs.Country)
                .Where(c => c.CountryCartoons.Any(cs => cs.CountryId == countryId));

            switch (sortBy?.ToLower())
            {
                case "date":
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;

                case "alphabet":
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            int totalCount = await query.CountAsync();

            var cartoons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cartoons, totalCount);
        }


        public async Task<(IEnumerable<Cartoon> Cartoons, int TotalCount)> GetPaginatedCartoonsByGenreIdAsync(int genreId, int pageNumber, int pageSize, string sortBy = "alphabet")
        {
            var query = _context.Cartoons
                .Include(c => c.Category)
                .Include(c => c.Ratings)
                .Include(c => c.CartoonGenres)
                .ThenInclude(cs => cs.Genre)
                .Where(c => c.CartoonGenres.Any(cs => cs.GenreId == genreId));

            switch (sortBy?.ToLower())
            {
                case "date":
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;

                case "alphabet":
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            int totalCount = await query.CountAsync();

            var cartoons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (cartoons, totalCount);
        }



    }
}

