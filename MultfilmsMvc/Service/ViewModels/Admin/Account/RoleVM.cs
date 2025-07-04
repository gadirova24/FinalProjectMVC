using System;
namespace Service.ViewModels.Admin.Account
{
	public class RoleVM
	{
        public string Name { get; set; }
        public UserVM[] Users { get; set; }
    }
}

