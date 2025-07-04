using System;
namespace Service.ViewModels.UI
{
    public class SidebarVM
    {
        public IEnumerable<StudioVM> Studios { get; set; }
        public IEnumerable<CollectionVM> Collections { get; set; }
        public IEnumerable<GenreVM> Genres { get; set; }
        public IEnumerable<CountryVM> Countries { get; set; }
    }
}

