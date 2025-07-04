using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.Admin.Collection
{
	public class CollectionEditVM
	{
		public  string Name { get; set; }
		public string ExistImage { get; set; }
		public IFormFile UploadImage { get; set; }
	}
    public class CollectionEditVMValidator : AbstractValidator<CollectionEditVM>
    {
        public CollectionEditVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Name must be  between 4 and 50 characters");

            RuleFor(x => x.UploadImage)
             .NotEmpty().WithMessage("Image is required");
        }
    }
}

