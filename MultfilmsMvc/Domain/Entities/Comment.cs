using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Comment:BaseEntity
	{
        public int CartoonId { get; set; }
        public Cartoon Cartoon { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; } 
        public int Rating { get; set; }
    }
}

