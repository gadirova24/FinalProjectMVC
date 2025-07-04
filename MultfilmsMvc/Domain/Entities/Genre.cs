using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Genre:BaseEntity
	{
		public string Name { get; set; }
		public ICollection<CartoonGenre> CartoonGenres { get; set; }
	}
}

