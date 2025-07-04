using System;
using FluentValidation;
using Service.ViewModels.Admin.Category;

namespace Service.ViewModels.Admin.Genre
{
	public class GenreEditVM
	{
		public string Name { get; set; }
	}
    public class GenreEditVMValidator : AbstractValidator<GenreEditVM>
    {
        public GenreEditVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Name must be  between 2 and 50 characters");


        }
    }
}

