using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CartoonGenre:BaseEntity
	{
		public int CartoonId { get; set; }
		public Cartoon Cartoon { get; set; }
		public int GenreId { get; set; }
		public Genre Genre { get; set; }
	
	}
}

