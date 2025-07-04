using System;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.CartoonSlider;
using Service.ViewModels.UI;
using static Service.ViewModels.Admin.CartoonSlider.AdminCartoonSliderVM;

namespace Service.Services
{
    public class CartoonSliderService : ICartoonSliderService
    {
        private readonly ICartoonSliderRepository _cartoonSliderRepo;
        private readonly ICartoonRepository _cartoonRepo;  
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CartoonSliderService(
            ICartoonSliderRepository cartoonSliderRepo,
            ICartoonRepository cartoonRepo,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _cartoonSliderRepo = cartoonSliderRepo;
            _cartoonRepo = cartoonRepo;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<CartoonSliderVM>> GetAllAsync()
        {
            var sliders = await _cartoonSliderRepo.Query()
                .Include(c => c.Cartoon)
                .ThenInclude(cartoon => cartoon.Ratings)
                .Include(c => c.Cartoon)
                .ThenInclude(cartoon => cartoon.CartoonGenres)
                .ThenInclude(cg => cg.Genre)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartoonSliderVM>>(sliders);
        }

        public async Task<IEnumerable<AdminCartoonSliderVM>> GetAllAdminAsync()
        {
            var sliders = await _cartoonSliderRepo.Query()
                .Include(x => x.Cartoon)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AdminCartoonSliderVM>>(sliders);
        }

        public async Task<CartoonSliderVM> GetByIdAsync(int id)
        {
            var slider = await _cartoonSliderRepo.Query()
                .Include(x => x.Cartoon)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (slider == null) throw new NotFoundException("Slider not found");
            return _mapper.Map<CartoonSliderVM>(slider);
        }

        public async Task CreateAsync(CartoonSliderCreateVM model)
        {
         

            if (!model.BackgroundImageFile.CheckFileType("image/"))
                throw new ValidationException("Image type must be only image");

            if (!model.BackgroundImageFile.CheckFileSize(500))
                throw new ValidationException("Image size must be smaller than 500KB");

            var cartoon = await _cartoonRepo.GetByIdAsync(model.CartoonId);  
            if (cartoon == null)
                throw new NotFoundException("Cartoon not found");

            string fileName = Guid.NewGuid() + Path.GetExtension(model.BackgroundImageFile.FileName);
            string filePath = _env.GenerateFilePath("assets/images/cartoons", fileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await model.BackgroundImageFile.CopyToAsync(stream);
            }

            var entity = _mapper.Map<CartoonSlider>(model);
            entity.BackgroundImage = fileName;

            await _cartoonSliderRepo.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, CartoonSliderEditVM model)
        {
            var slider = await _cartoonSliderRepo.GetByIdAsync(id);
            if (slider == null)
                throw new NotFoundException("Cartoon slider not found");

            slider.CartoonId = model.CartoonId;
    

            if (model.BackgroundImageFile != null)
            {
                if (!model.BackgroundImageFile.CheckFileType("image/"))
                    throw new ValidationException("Image type must be only image");

                if (!model.BackgroundImageFile.CheckFileSize(500))
                    throw new ValidationException("Image size must be smaller than 500KB");

                if (!string.IsNullOrWhiteSpace(slider.BackgroundImage))
                {
                    string oldPath = _env.GenerateFilePath("assets/images/cartoons", slider.BackgroundImage);
                    oldPath.DeleteFile();
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(model.BackgroundImageFile.FileName);
                string filePath = _env.GenerateFilePath("assets/images/cartoons", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await model.BackgroundImageFile.CopyToAsync(stream);
                }

                slider.BackgroundImage = fileName;
            }

            await _cartoonSliderRepo.UpdateAsync(slider);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _cartoonSliderRepo.GetByIdAsync(id);
            if (data == null)
                throw new NotFoundException("Slider not found");

            if (!string.IsNullOrEmpty(data.BackgroundImage))
            {
                string filePath = _env.GenerateFilePath("assets/images/cartoons", data.BackgroundImage);
                filePath.DeleteFile();
            }

            await _cartoonSliderRepo.DeleteAsync(data);
        }
    }

}

