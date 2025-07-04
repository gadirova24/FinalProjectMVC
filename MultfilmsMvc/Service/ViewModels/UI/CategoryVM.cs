using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class CategoryVM
	{
		public string Name { get; set; }
        public ICollection<Cartoon> Cartoons { get; set; }
    }
}

