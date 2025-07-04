using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Collection;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.UI;

namespace Service.Services
{
	public class CollectionService:ICollectionService
	{
        private readonly ICollectionRepository _collectionRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CollectionService(ICollectionRepository collectionRepo,
                                 IMapper mapper,
                                 IWebHostEnvironment env)
		{
            _collectionRepo = collectionRepo;
            _mapper = mapper;
            _env = env;
		}

        public async Task CreateAsync(CollectionCreateVM collection)
        {
     
                if (!collection.Image.CheckFileType("image/"))
                {
                    throw new ValidationException("Image type must be only image");
                }
                if (!collection.Image.CheckFileSize(500))
                {
                    throw new ValidationException("Image size must be smaller than 500KB");
                }
            
  
                string fileName = Guid.NewGuid().ToString() + " " + collection.Image.FileName;
                string filePath = _env.GenerateFilePath("assets/images/collections", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await collection.Image.CopyToAsync(stream);
                }
            var newCollection = _mapper.Map<Collection>(collection);
            newCollection.Image = fileName;

            await _collectionRepo.CreateAsync(newCollection);
        }
        public async Task<CollectionDetailVM> GetCollectionDetailAsync(int id)
        {
            var collection = await _collectionRepo.Query()
                .Include(s => s.CartoonCollections)
                    .ThenInclude(cs => cs.Cartoon)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (collection == null) return null;

            var cartoonNames = collection.CartoonCollections
                .Select(cs => cs.Cartoon.Name)
                .ToList();

            return new CollectionDetailVM
            {
                Name = collection.Name,
                Image = collection.Image,
                CartoonCount = cartoonNames.Count,
                CartoonNames = cartoonNames
            };
        }
        public async Task DeleteAsync(int id)
        {
            var data = await _collectionRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            if (!string.IsNullOrEmpty(data.Image))
            {
                string filePath = _env.GenerateFilePath("assets/images/collections", data.Image);
                filePath.DeleteFile();
            }
            await _collectionRepo.DeleteAsync(data);
        }

        public async Task<IEnumerable<AdminCollectionVM>> GetAllAdminAsync()
        {
            return _mapper.Map<IEnumerable<AdminCollectionVM>>(await _collectionRepo.GetAllAsync());
        }

        public async Task<bool> IsDuplicateAsync(string name, string imageFileName)
        {
            return await _collectionRepo.ExistsByNameAndImageAsync(name, imageFileName);
        }
        public async Task<IEnumerable<CollectionVM>> GetAllAsync()
        {
            var collections = await _collectionRepo.Query()
                                    .Include(c => c.CartoonCollections)
                                    .ToListAsync();
            return _mapper.Map<IEnumerable<CollectionVM>>(collections);
        }


        public async Task<AdminCollectionVM> GetByIdAsync(int id)
        {
            var data = await _collectionRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminCollectionVM>(data);
        }

        public async Task<IEnumerable<CollectionVM>> GetCollectionsWithCartoonsAsync()
        {
            var collections = await _collectionRepo.GetCollectionsWithCartoonsAsync();
            return _mapper.Map<IEnumerable<CollectionVM>>(collections);
        }

        public async Task UpdateAsync(int id, CollectionEditVM collection)
        {
            var existData = await _collectionRepo.GetByIdAsync(id);
            if (existData is null) throw new NotFoundException("Collection not found");
            _mapper.Map(collection, existData);
            if (collection.UploadImage is not null)
            {
                if (!string.IsNullOrWhiteSpace(existData.Image))
                {
                    string oldFilePath = _env.GenerateFilePath("assets/images/collections", existData.Image);
                    oldFilePath.DeleteFile();
                }
                string fileName = Guid.NewGuid().ToString() + "-" + collection.UploadImage.FileName;
                string newFilePath = _env.GenerateFilePath("assets/images/collections", fileName);
                using (FileStream stream = new(newFilePath, FileMode.Create))
                {
                    await collection.UploadImage.CopyToAsync(stream);
                }

                existData.Image = fileName;
            }

            await _collectionRepo.UpdateAsync(existData);
        }
    }
}

