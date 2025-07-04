using System;
namespace Service.ViewModels.Admin.Cartoon
{
	public class AdminCartoonVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string PlayerUrl { get; set; }
        public string Plot { get; set; }
        public int Year { get; set; }
        public string CategoryName { get; set; }
    }
}

