using System;
namespace Service.ViewModels.Admin.Genre
{
	public class GenreDetailVM
	{
		public string Name { get; set; }
		public int CartoonCount { get; set; }
        public List<string> CartoonNames { get; set; } = new();
    }
}

