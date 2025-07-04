using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.ViewModels.Admin.CartoonSlider
{
	public class CartoonSliderCreateVM
	{
        public int CartoonId { get; set; }
        public IFormFile BackgroundImageFile { get; set; }
        public List<SelectListItem>? CartoonOptions { get; set; }
    }
}

