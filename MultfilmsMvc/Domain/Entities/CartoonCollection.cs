using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CartoonCollection:BaseEntity
	{
		public int CartoonId { get; set; }
		public Cartoon Cartoon { get; set; }
		public int CollectionId { get; set; }
		public Collection Collection { get; set; }


	}
}

