using System;
namespace Service.ViewModels.UI
{
    public class CartoonDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Plot { get; set; }
        public int Year { get; set; }
        public string CategoryName { get; set; }
        public List<string> StudioNames { get; set; }
        public List<string> CountryNames { get; set; }
        public List<string> GenreNames { get; set; }
        public string PlayerUrl { get; set; }
        public double AverageRating { get; set; }
        public List<string> DirectorNames { get; set; }
        public List<string> ActorNames { get; set; }
        public bool IsFavorite { get; set; } = false;
    }
}

