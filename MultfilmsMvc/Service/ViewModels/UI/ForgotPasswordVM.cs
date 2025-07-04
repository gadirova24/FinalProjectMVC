using System;
using FluentValidation;

namespace Service.ViewModels.UI
{
	public class ForgotPasswordVM
	{
        public string Email { get; set; }
    }
    public class ForgotPasswordVMValidator : AbstractValidator<ForgotPasswordVM>
    {
        public ForgotPasswordVMValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}

