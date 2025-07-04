using System;
using FluentValidation;

namespace Service.ViewModels.Admin.Category
{
	public class CategoryEditVM
	{
        public string Name { get; set; }
        public class CategoryEditVMValidator : AbstractValidator<CategoryEditVM>
        {
            public CategoryEditVMValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .Length(4, 50).WithMessage("Name must be  between 4 and 50 characters");


            }
        }
    }
}

