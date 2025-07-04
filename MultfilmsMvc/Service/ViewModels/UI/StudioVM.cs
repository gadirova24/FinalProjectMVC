using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class StudioVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CartoonCount { get; set; }
        public ICollection<CartoonStudio> CartoonStudios { get; set; }
    }
}

