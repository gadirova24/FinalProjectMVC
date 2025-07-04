using System;
namespace Service.ViewModels.Admin.Person
{
	public class AdminPersonVM
	{
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }
}

