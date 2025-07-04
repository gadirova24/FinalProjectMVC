using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Collection:BaseEntity
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public ICollection<CartoonCollection> CartoonCollections { get; set; }
	
	}
}

