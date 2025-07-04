
using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetGenresWithCartoonsAsync()
        {
            return await _context.Genres
               .Where(s => s.CartoonGenres.Any())
               .ToListAsync();
        }
    }
}

