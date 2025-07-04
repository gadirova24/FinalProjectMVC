using System;
namespace Service.ViewModels.Admin.Cartoon
{
	public class AdminCartoonDetailVM
	{
        public string Name { get; set; }
        public string Image { get; set; }
        public string Plot { get; set; }
        public string PlayerUrl { get; set; }
        public int Year { get; set; }
        public string CategoryName { get; set; }
        public List<string> StudioNames { get; set; } = new();
        public List<string> CollectionNames { get; set; } = new();
        public List<string> CountryNames { get; set; } = new();
        public List<string> GenreNames { get; set; } = new();

        public List<string> ActorNames { get; set; } = new();
        public List<string> DirectorNames { get; set; } = new();

        public double AverageRating { get; set; }
    }
}

