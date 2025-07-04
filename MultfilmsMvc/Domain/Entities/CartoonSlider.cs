
using System;
using Domain.Common;

namespace Domain.Entities
{
	public class CartoonSlider:BaseEntity
	{
        public int CartoonId { get; set; }
        public Cartoon Cartoon { get; set; }
        public string BackgroundImage { get; set; }
    }
}

