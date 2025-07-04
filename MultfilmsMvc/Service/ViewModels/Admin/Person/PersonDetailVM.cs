using System;
namespace Service.ViewModels.Admin.Person
{
	public class PersonDetailVM
	{
        public string FullName { get; set; }
        public List<string> Roles { get; set; } = new();
        public int CartoonCount { get; set; }
        public List<string> CartoonNames { get; set; } = new();
    }
}

