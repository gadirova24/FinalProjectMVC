    using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Rating:BaseEntity
	{
        public int Value { get; set; } 
        public int CartoonId { get; set; }
        public Cartoon Cartoon { get; set; }

        public string UserId { get; set; } 
        public AppUser User { get; set; }
    }
}

