using System;
using FluentValidation;

namespace Service.ViewModels.UI
{
	public class RegisterVM
	{
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidator()
        {
            RuleFor(x => x.FullName)
               .NotNull().WithMessage("Name is required")
               .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.UserName)
             .NotNull().WithMessage("UserName is required");
            RuleFor(x => x.Email)
              .NotNull().WithMessage("Email is required")
              .EmailAddress().WithMessage("Email format is wrong")
             .NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password)
       .NotNull().WithMessage("Password is required")
       .NotEmpty().WithMessage("Password is required")
       .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
     
            RuleFor(x => x.ConfirmPassword)
                .NotNull().WithMessage("Confirm Password is required")
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

        }
    }
}

