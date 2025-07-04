using System;
using Domain.Helpers.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.Admin.Genre;

namespace Service.ViewModels.Admin.Person
{
	public class PersonCreateVM
	{
        public string FullName { get; set; }
        public PersonRoleType? SelectedRole { get; set; }
        public List<SelectListItem> RoleOptions { get; set; } = new List<SelectListItem>();
    }
    public class PersonCreateVMValidator : AbstractValidator<PersonCreateVM>
    {
        public PersonCreateVMValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .Length(5, 50).WithMessage("FullName must be  between 5 and 50 characters");

            RuleFor(x => x.SelectedRole)
          .NotNull().WithMessage("At least one role must be selected");
        }
    }
}

