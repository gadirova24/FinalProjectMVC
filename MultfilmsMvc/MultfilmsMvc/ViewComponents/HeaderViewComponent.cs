using System;
using Microsoft.AspNetCore.Mvc;

namespace MultfilmsMvc.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

