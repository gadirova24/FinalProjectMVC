using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.Admin.Genre;
using Service.ViewModels.UI;

namespace Service.Services
{
    public class GenreService : IGenreService
    {

        private readonly IGenreRepository _genreRepo;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository genreRepo,
                                IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;

        }
        public async Task CreateAsync(GenreCreateVM genre)
        {
            await _genreRepo.CreateAsync(_mapper.Map<Genre>(genre));
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _genreRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            await _genreRepo.DeleteAsync(data);
        }

        public async Task<IEnumerable<AdminGenreVM>> GetAllAdminAsync()
        {
            return _mapper.Map<IEnumerable<AdminGenreVM>>(await _genreRepo.GetAllAsync());
        }

        public async Task<IEnumerable<GenreVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<GenreVM>>(await _genreRepo.GetAllAsync());
        }

        public async Task<AdminGenreVM> GetByIdAsync(int id)
        {
            var data = await _genreRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminGenreVM>(data);
        }

        public async Task UpdateAsync(int id, GenreEditVM genre)
        {
            var existData = await _genreRepo.GetByIdAsync(id);
            _mapper.Map(genre, existData);
            await _genreRepo.UpdateAsync(_mapper.Map<Genre>(existData));
        }
        public async Task<GenreDetailVM> GetGenreDetailAsync(int id)
        {
            var genre = await _genreRepo.Query()
                .Include(s => s.CartoonGenres)
                    .ThenInclude(cs => cs.Cartoon)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (genre == null) return null;

            var cartoonNames = genre.CartoonGenres
                .Select(cs => cs.Cartoon.Name)
                .ToList();

            return new GenreDetailVM
            {
                Name = genre.Name,
                CartoonCount = cartoonNames.Count,
                CartoonNames = cartoonNames
            };
        }

        public async Task<IEnumerable<GenreVM>> GetGenresWithCartoonsAsync()
        {
            var genres = await _genreRepo.GetGenresWithCartoonsAsync();
            return _mapper.Map<IEnumerable<GenreVM>>(genres);
        }
    }
}

