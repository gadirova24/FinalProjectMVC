using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Studio:BaseEntity
	{
        public string Name { get; set; }
        public string Image { get; set; }
		public ICollection<CartoonStudio> CartoonStudios { get; set; }
	}
}

