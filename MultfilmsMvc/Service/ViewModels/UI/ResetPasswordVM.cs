using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Service.ViewModels.UI
{
	public class ResetPasswordVM
	{
        public string Token { get; set; }
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordVMValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordVMValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
        }
    }
}

