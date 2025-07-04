using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CartoonStudio:BaseEntity
	{
		public int StudioId { get; set; }
		public Studio Studio { get; set; }
		public int CartoonId { get; set; }
		public Cartoon Cartoon { get; set; }
		
	}
}

