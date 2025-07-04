using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Repository.Repositories.Interfaces;

namespace Service.ViewModels.Admin.Country
{
	public class CountryEditVM
	{
		public string Name { get; set; }
        public string ExistImage { get; set; }
        public IFormFile UploadImage { get; set; }

    }
    public class CountryEditVMValidator : AbstractValidator<CountryEditVM>
    {
        public CountryEditVMValidator(ICountryRepository _countryRepo)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be  between 2 and 50 characters");

            RuleFor(x => x.UploadImage)
      .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.ExistImage))
      .WithMessage("Image is required");


           
        }
    }
}

