using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Repository.Repositories.Interfaces;

namespace Service.ViewModels.Admin.Country
{
	public class CountryCreateVM
	{
        public IFormFile Image { get; set; }
        public string Name { get; set; }
    }
    public class CountryCreateVMValidator : AbstractValidator<CountryCreateVM>
    {
        public CountryCreateVMValidator(ICountryRepository _countryRepo)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be  between 2 and 50 characters");

            RuleFor(x => x.Image)
             .NotEmpty().WithMessage("Image is required");


           
        }
    }
}

