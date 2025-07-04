using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Admin.CartoonSlider
{
	public class CartoonSliderCreateVM
	{
        public int CartoonId { get; set; }
        public IFormFile BackgroundImageFile { get; set; }
    }
}

