
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Service.Helpers.Enums;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Admin.Category;
using Service.ViewModels.Admin.Country;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        public CountryController(ICountryService countryService,
                                 ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            _logger.LogInformation("Admin requested country list at {Time}", DateTime.UtcNow);
            return View(await _countryService.GetAllAdminAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var detail = await _countryService.GetCountryDetailAsync(id.Value);
            if (detail == null) return NotFound();

            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(CountryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            var isDuplicate = await _countryService.IsDuplicateAsync(request.Name, request.Image.FileName);
            if (isDuplicate)
            {
                _logger.LogWarning("Duplicate country detected: {Name}", request.Name);
                ModelState.AddModelError("", "Country with this name exist.");
                return View(request);
            }

            await _countryService.CreateAsync(request);
            _logger.LogInformation("Country created successfully: {Name}", request.Name);
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
           await _countryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var country = await _countryService.GetByIdAsync(id.Value);
            if (country is null) return NotFound();
            return View(new CountryEditVM
            {
                ExistImage = country.Image,
                Name = country.Name

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CountryEditVM request)
        {
            try
            {
                await _countryService.UpdateAsync(id, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update failed");
                ModelState.AddModelError("", "Failed to update country.");
                var country = await _countryService.GetByIdAsync(id);
                request.ExistImage = country.Image;
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}

