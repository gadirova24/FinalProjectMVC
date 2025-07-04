using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class CollectionVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CartoonCount { get; set; }
        public ICollection<CartoonCollection> CartoonCollections { get; set; }
    }
}

