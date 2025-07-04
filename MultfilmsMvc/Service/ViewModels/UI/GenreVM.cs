using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class GenreVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CartoonCount { get; set; }
        public ICollection<CartoonGenre> CartoonGenres { get; set; }
    }
}

