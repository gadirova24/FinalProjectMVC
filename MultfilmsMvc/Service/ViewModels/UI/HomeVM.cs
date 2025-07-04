using System;
namespace Service.ViewModels.UI
{
	public class HomeVM
	{
		public IEnumerable<CountryVM> Countries { get; set; }
        public IEnumerable<CollectionVM> Collections { get; set; }
        public IEnumerable<StudioVM> Studios { get; set; }
        public IEnumerable<GenreVM> Genres { get; set; }
        public IEnumerable<PersonVM> Persons { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<CartoonVM> Cartoons { get; set; }
        public CartoonListVM CartoonList { get; set; }
        public IEnumerable<CartoonVM> TopRated { get; set; }
        public IEnumerable<CartoonSliderVM> CartoonSliders { get; set; }
    }
}

