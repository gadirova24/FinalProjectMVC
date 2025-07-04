using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.ViewModels.Admin.Country;

namespace Service.ViewModels.Admin.Studio
{
	public class StudioCreateVM
	{
		public string Name { get; set; }
		public IFormFile Image{ get; set; }
        public class StudioCreateVMValidator : AbstractValidator<StudioCreateVM>
        {
            public StudioCreateVMValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .Length(2, 50).WithMessage("Name must be  between 2 and 50 characters");

                RuleFor(x => x.Image)
                 .NotEmpty().WithMessage("Image is required");
            }
        }
    }
}

