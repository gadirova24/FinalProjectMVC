using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CountryCartoon:BaseEntity
	{
		public int CartoonId { get; set; }
		public Cartoon Cartoon { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
	}
}

