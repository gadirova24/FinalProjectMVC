using System;
using FluentValidation;
using Service.ViewModels.Admin.Collection;

namespace Service.ViewModels.Admin.Category
{
	public class CategoryCreateVM
	{
		public string Name { get; set; }
	}
    public class CategoryCreateVMValidator : AbstractValidator<CategoryCreateVM>
    {
        public CategoryCreateVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Name must be  between 4 and 50 characters");

           
        }
    }
}

