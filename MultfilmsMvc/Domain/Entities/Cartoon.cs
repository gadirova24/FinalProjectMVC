using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Cartoon:BaseEntity
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string Plot { get; set; }
		public int Year { get; set; }
		public string PlayerUrl { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
        public ICollection<CountryCartoon> CountryCartoons { get; set; }
        public ICollection<CartoonStudio> CartoonStudios { get; set; }
        public ICollection<CartoonCollection> CartoonCollections { get; set; }
        public ICollection<CartoonGenre> CartoonGenres { get; set; }
        public ICollection<CartoonDirector> Directors { get; set; }
        public ICollection<CartoonActor> Actors { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }


    }
}

