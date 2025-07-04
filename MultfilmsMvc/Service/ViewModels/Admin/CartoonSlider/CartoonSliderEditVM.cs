using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.ViewModels.Admin.CartoonSlider
{
	public class CartoonSliderEditVM
	{
        public int Id { get; set; }
        public int CartoonId { get; set; }
        public IFormFile BackgroundImageFile { get; set; }
        public string ExistingImage { get; set; }
        public List<SelectListItem>? CartoonOptions { get; set; }
    }
  
}

