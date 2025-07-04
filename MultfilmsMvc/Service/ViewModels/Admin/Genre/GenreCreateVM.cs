using System;
using FluentValidation;
using Service.ViewModels.Admin.Category;

namespace Service.ViewModels.Admin.Genre
{
	public class GenreCreateVM
	{
		public string Name { get; set; }
	}
    public class GenreCreateVMValidator : AbstractValidator<GenreCreateVM>
    {
        public GenreCreateVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Name must be  between 4 and 50 characters");


        }
    }
}

