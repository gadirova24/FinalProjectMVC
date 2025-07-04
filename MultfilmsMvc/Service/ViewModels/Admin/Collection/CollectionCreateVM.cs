using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.ViewModels.Admin.Country;

namespace Service.ViewModels.Admin.Collection
{
	public class CollectionCreateVM
	{
		public string Name { get; set; }
		public IFormFile Image { get; set; }
	}
    public class CollectionCreateVMValidator : AbstractValidator<CollectionCreateVM>
    {
        public CollectionCreateVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Name must be  between 4 and 50 characters");

            RuleFor(x => x.Image)
             .NotEmpty().WithMessage("Image is required");
        }
    }
}

