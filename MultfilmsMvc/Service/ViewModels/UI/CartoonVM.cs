using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class CartoonVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Plot { get; set; }
        public int Year { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int? UserRating { get; set; }
        public ICollection<CountryCartoon> CountryCartoons { get; set; }
        public ICollection<CartoonStudio> CartoonStudios { get; set; }
        public ICollection<CartoonCollection> CartoonCollections { get; set; }
        public ICollection<CartoonGenre> CartoonGenres { get; set; }
        public ICollection<CartoonDirector> Directors { get; set; }
        public ICollection<CartoonActor> Actors { get; set; }


    }
}

