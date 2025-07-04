using System;
namespace Service.ViewModels.Admin.Account
{
	public class UserVM
	{
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}

