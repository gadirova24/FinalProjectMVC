using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Country;
using Service.ViewModels.Admin.Studio;
using Service.ViewModels.UI;

namespace Service.Services
{
	public class CountryService:ICountryService
	{
		private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public CountryService(ICountryRepository countryRepo,
                              IMapper mapper,
                               IWebHostEnvironment env)
		{
            _countryRepo = countryRepo;
            _mapper = mapper;
            _env = env;
		}

        public async Task CreateAsync(CountryCreateVM country)
        {
          
                if (!country.Image.CheckFileType("image/"))
                {
                    throw new ValidationException("Image type must be only image");
                }
                if (!country.Image.CheckFileSize(500))
                {
                    throw new ValidationException("Image size must be smaller than 500KB");
                }
         


            string fileName = Guid.NewGuid().ToString() + " " + country.Image.FileName;


            string filePath = _env.GenerateFilePath("assets/images/countries", fileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await country.Image.CopyToAsync(stream);
                }
            
            var newCountry = _mapper.Map<Country>(country);
           newCountry.Image = fileName;

            await _countryRepo.CreateAsync(newCountry);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _countryRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            if (!string.IsNullOrEmpty(data.Image))
            {
                string filePath = _env.GenerateFilePath("assets/images/countries", data.Image);
                filePath.DeleteFile();
            }
            await _countryRepo.DeleteAsync(data);
        }

        public async Task<IEnumerable<CountryVM>> GetAllAsync()
        {
            var countries = await _countryRepo.Query()
                                             .Include(c => c.CountryCartoons)
                                             .ToListAsync();

            return _mapper.Map<IEnumerable<CountryVM>>(countries);
          
        }
        public async Task<CountryDetailVM> GetCountryDetailAsync(int id)
        {
            var country = await _countryRepo.Query()
                .Include(s => s.CountryCartoons)
                    .ThenInclude(cs => cs.Cartoon)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (country == null) return null;

            var cartoonNames = country.CountryCartoons
                .Select(cs => cs.Cartoon.Name)
                .ToList();

            return new CountryDetailVM
            {
                Name = country.Name,
                Image = country.Image,
                CartoonCount = cartoonNames.Count,
                CartoonNames = cartoonNames
            };
        }
        public async Task<bool> IsDuplicateAsync(string name, string imageFileName)
        {
            return await _countryRepo.ExistsByNameAndImageAsync(name, imageFileName);
        }
        public async Task<AdminCountryVM> GetByIdAsync(int id)
        {
            var data = await _countryRepo.GetByIdAsync(id);

            if (data is null)
            {
                throw new NotFoundException("Not found.");
            }
            return _mapper.Map<AdminCountryVM>(data);
        }
        public async Task UpdateAsync(int id, CountryEditVM country)
        {
            var existData = await _countryRepo.GetByIdAsync(id);
            if (existData is null) throw new Exception("Country not found");
            existData.Name = country.Name;
            if (country.UploadImage is not null)
            {
                if (!string.IsNullOrWhiteSpace(existData.Image))
                {
                    string oldFilePath = _env.GenerateFilePath("assets/images/countries", existData.Image);
                    oldFilePath.DeleteFile(); 
                }
                string fileName = Guid.NewGuid().ToString() + "-" + country.UploadImage.FileName;
                string newFilePath = _env.GenerateFilePath("assets/images/countries", fileName);
                using (FileStream stream = new(newFilePath, FileMode.Create))
                {
                    await country.UploadImage.CopyToAsync(stream);
                }

                existData.Image = fileName;
            }

            await _countryRepo.UpdateAsync(existData);
        }
        public async Task<IEnumerable<AdminCountryVM>> GetAllAdminAsync()
            {
                return _mapper.Map<IEnumerable<AdminCountryVM>>(await _countryRepo.GetAllAsync());
            }

        public async Task<IEnumerable<CountryVM>> GetCountriesWithCartoonsAsync()
        {
            var countries = await _countryRepo.GetCountriesWithCartoonsAsync();
            return _mapper.Map<IEnumerable<CountryVM>>(countries);
        }
    }
}

