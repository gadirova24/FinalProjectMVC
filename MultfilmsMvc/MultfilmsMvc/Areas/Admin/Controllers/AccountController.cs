using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Enums;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users); 
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var roles = await _accountService.GetAllRolesAsync();
            return View(roles); 
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(UserRoleVM model)
        {
            try
            {
                await _accountService.AddRoleToUserAsync(model);
                TempData["SuccessMessage"] = "Role succesfully added.";
                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Users));
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(UserRoleVM model)
        {
            try
            {
                await _accountService.RemoveRoleFromUserAsync(model);
                TempData["SuccessMessage"] = "Role succesfully deleted.";
                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Users));
            }
        }
        //createrole
        //[HttpGet]
        //public async Task<ActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRolesAsync();
        //    return Ok();

        //}
    }
}

