using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Studio;
using Service.ViewModels.UI;

namespace Service.Services
{
	public class StudioService:IStudioService
	{
        private readonly IStudioRepository _studioRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public StudioService(IStudioRepository studioRepo,
                            IMapper mapper,
                            IWebHostEnvironment env)
		{
            _studioRepo = studioRepo;
            _mapper = mapper;
            _env = env;
		}

        public async Task<IEnumerable<StudioVM>> GetAllAsync()
        {
            var studios = await _studioRepo.Query()
                             .Include(c => c.CartoonStudios)
                             .ToListAsync();
            return _mapper.Map<IEnumerable<StudioVM>>(studios);
        }
        public async Task<IEnumerable<AdminStudioVM>> GetAllAdminAsync()
        {
            var studios = await _studioRepo.Query()
                                    .Include(c => c.CartoonStudios)
                                    .ToListAsync();
            return _mapper.Map<IEnumerable<AdminStudioVM>>(studios);
        }

        public async Task<AdminStudioVM> GetByIdAsync(int id)
        {
            var data = await _studioRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminStudioVM>(data);
        }
        public async Task<bool> IsDuplicateAsync(string name, string imageFileName)
        {
            return await _studioRepo.ExistsByNameAndImageAsync(name, imageFileName);
        }
        public async Task<StudioDetailVM> GetStudioDetailAsync(int id)
        {
            var studio = await _studioRepo.Query()
                .Include(s => s.CartoonStudios)
                    .ThenInclude(cs => cs.Cartoon)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studio == null) return null;

            var cartoonNames = studio.CartoonStudios
                .Select(cs => cs.Cartoon.Name)
                .ToList();

            return new StudioDetailVM
            {
                Name = studio.Name,
                Image = studio.Image,
                CartoonCount = cartoonNames.Count,
                CartoonNames = cartoonNames
            };
        }
        public async Task CreateAsync(StudioCreateVM studio)
        {
         
                if (!studio.Image.CheckFileType("image/"))
                {
                    throw new ValidationException("Image type must be only image");
                }
                if (!studio.Image.CheckFileSize(500))
                {
                    throw new ValidationException("Image size must be smaller than 500KB");
                }
        
                string fileName = Guid.NewGuid().ToString() + " " + studio.Image.FileName;
                string filePath = _env.GenerateFilePath("assets/images/studios", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await studio.Image.CopyToAsync(stream);
                }


            
            var newStudio = _mapper.Map<Studio>(studio);
            newStudio.Image = fileName;

            await _studioRepo.CreateAsync(newStudio);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _studioRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            if (!string.IsNullOrEmpty(data.Image))
            {
                string filePath = _env.GenerateFilePath("assets/images/studios", data.Image);
                filePath.DeleteFile();
            }
            await _studioRepo.DeleteAsync(data);
        }

        public async Task UpdateAsync(int id, StudioEditVM country)
        {
            var existData = await _studioRepo.GetByIdAsync(id);
            if (existData is null) throw new Exception("Studio not found");
            _mapper.Map(country, existData);
            if (country.UploadImage is not null)
            {
                if (!string.IsNullOrWhiteSpace(existData.Image))
                {
                    string oldFilePath = _env.GenerateFilePath("assets/images/studios", existData.Image);
                    oldFilePath.DeleteFile();
                }
                string fileName = Guid.NewGuid().ToString() + "-" + country.UploadImage.FileName;
                string newFilePath = _env.GenerateFilePath("assets/images/studios", fileName);
                using (FileStream stream = new(newFilePath, FileMode.Create))
                {
                    await country.UploadImage.CopyToAsync(stream);
                }

                existData.Image = fileName;
            }

            await _studioRepo.UpdateAsync(existData);
        }
        public async Task<IEnumerable<StudioVM>> GetStudiosWithCartoonsAsync()
        {
            var studios = await _studioRepo.GetStudiosWithCartoonsAsync();
            return _mapper.Map<IEnumerable<StudioVM>>(studios);
        }
    }
}

