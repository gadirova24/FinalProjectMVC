using System;
using Domain.Helpers.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.ViewModels.Admin.Person
{
	public class PersonEditVM
	{
        public string FullName { get; set; }
        public List<PersonRoleType> SelectedRoles { get; set; } = new();
        public List<SelectListItem> RoleOptions { get; set; } = new();
    }
}

