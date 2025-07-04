using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Favorite:BaseEntity
	{
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CartoonId { get; set; }
        public Cartoon Cartoon { get; set; }
    }
}

