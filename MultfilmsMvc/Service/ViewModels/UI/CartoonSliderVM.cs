using System;
namespace Service.ViewModels.UI
{
	public class CartoonSliderVM
	{
        public int Id { get; set; }
        public int CartoonId { get; set; }
        public string CartoonName { get; set; }
        public string BackgroundImage { get; set; } 
        public string Plot { get; set; }
        public double AverageRating { get; set; }
        public List<string> GenreNames { get; set; }
        public int Year { get; set; }
    }
}

