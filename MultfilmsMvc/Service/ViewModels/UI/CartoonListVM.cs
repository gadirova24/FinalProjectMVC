using System;
namespace Service.ViewModels.UI
{
    public class CartoonListVM
    {
        public IEnumerable<CartoonVM> Cartoons { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
     
    }
}

