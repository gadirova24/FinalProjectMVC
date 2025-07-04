using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CartoonActor : BaseEntity
	{
		public int CartoonId { get; set; }
		public Cartoon Cartoon { get; set; }
		public int PersonId { get; set; }
		public Person Person { get; set; }
	}
}

