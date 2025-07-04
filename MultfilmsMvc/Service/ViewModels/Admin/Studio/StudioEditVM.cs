using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.Admin.Studio
{
	public class StudioEditVM
	{
		public string Name { get; set; }
		public string ExistImage { get; set; }
		public IFormFile UploadImage { get; set; }
	}
    public class StudioEditVMValidator : AbstractValidator<StudioEditVM>
    {
        public StudioEditVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(3, 50).WithMessage("Name must be  between 3 and 50 characters");

            RuleFor(x => x.UploadImage)
           .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.ExistImage))
            .WithMessage("Image is required");

        }
    }
}

