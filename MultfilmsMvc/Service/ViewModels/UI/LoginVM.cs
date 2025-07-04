using System;
using FluentValidation;

namespace Service.ViewModels.UI
{
	    public class LoginVM
	    {
            public string UsernameOrEmail { get; set; }
            public string Password { get; set; }
            public class LoginVMValidator : AbstractValidator<LoginVM>
            {
                public LoginVMValidator()
                {
                    RuleFor(x => x.UsernameOrEmail)
                       .NotNull().WithMessage("Can't be empty")
                       .NotEmpty().WithMessage("Can't be empty");

                RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

            }
            }
        }
}

