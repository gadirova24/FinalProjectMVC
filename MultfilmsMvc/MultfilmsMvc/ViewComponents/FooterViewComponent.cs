using System;
using Microsoft.AspNetCore.Mvc;

namespace MultfilmsMvc.ViewComponents
{
	public class FooterViewComponent:ViewComponent
	{
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

