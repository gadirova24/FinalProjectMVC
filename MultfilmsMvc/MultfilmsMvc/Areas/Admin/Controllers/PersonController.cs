using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Helpers.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Helpers.Enums;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Person;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        { 
            return View(await _personService.GetAllAdminAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PersonCreateVM
            {
                RoleOptions = Enum.GetValues(typeof(PersonRoleType))
                    .Cast<PersonRoleType>()
                    .Select(role => new SelectListItem
                    {
                        Value = ((int)role).ToString(),
                        Text = role.ToString()
                    }).ToList()
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                request.RoleOptions = Enum.GetValues(typeof(PersonRoleType))
                    .Cast<PersonRoleType>()
                    .Select(role => new SelectListItem
                    {
                        Value = ((int)role).ToString(),
                        Text = role.ToString()
                    }).ToList();

                return View(request);
            }

            await _personService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _personService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _personService.GetEditModelAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleOptions = Enum.GetValues(typeof(PersonRoleType))
                    .Cast<PersonRoleType>()
                    .Select(r => new SelectListItem
                    {
                        Text = r.ToString(),
                        Value = ((int)r).ToString(),
                        Selected = model.SelectedRoles.Contains(r)
                    }).ToList();

                return View(model);
            }

            try
            {
                var updated = await _personService.UpdateAsync(id, model);
                TempData["Success"] = updated ? "Update." : "No changes.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var detail = await _personService.GetDetailAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }
    }
}

