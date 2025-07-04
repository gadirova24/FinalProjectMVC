using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Country:BaseEntity
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public ICollection<CountryCartoon> CountryCartoons { get; set; }
	}
}

