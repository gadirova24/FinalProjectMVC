using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Service.ViewModels.Admin.CartoonSlider
{
	public class CartoonSliderEditVM
	{
        public int Id { get; set; }
        public int CartoonId { get; set; }
        public IFormFile BackgroundImageFile { get; set; }
        public string ExistingImage { get; set; }
    }
    public class CartoonSliderEditVMValidator : AbstractValidator<CartoonSliderEditVM>
    {
        public CartoonSliderEditVMValidator()
        {
            RuleFor(x => x.CartoonId)
          .NotEmpty()
          .WithMessage("Cartoon Id is required.");

            RuleFor(x => x.BackgroundImageFile)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.ExistingImage))
                .WithMessage("Background image is required if no existing image is present.");
        }
    }
}

