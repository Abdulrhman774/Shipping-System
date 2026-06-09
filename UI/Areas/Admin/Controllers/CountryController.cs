using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.Country;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CountryController : BaseController
    {
        // Constructor inject ICountryService (commented out as per instructions)
        /*
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        */

        // GET: Admin/Country
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample countries for display
            var countries = new List<TbCountry>
            {
                new TbCountry
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"),
                    CountryAname = "مصر",
                    CountryEname = "Egypt",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 1, 10, 0, 0)
                },
                new TbCountry
                {
                    Id = Guid.Parse("b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e"),
                    CountryAname = "المملكة العربية السعودية",
                    CountryEname = "Saudi Arabia",
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 2, 15, 14, 30, 0)
                },
                new TbCountry
                {
                    Id = Guid.Parse("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                    CountryAname = "الولايات المتحدة",
                    CountryEname = "United States",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 3, 10, 9, 15, 0)
                }
            };

            return View(countries);
        }

        // GET: Admin/Country/Create
        public IActionResult Create()
        {
            return View(new CreateCountryDto());
        }

        // POST: Admin/Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCountryDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Country/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            // Hardcoded update model with pre-filled values
            var dto = new UpdateCountryDto
            {
                CountryAname = "مصر",
                CountryEname = "Egypt"
            };
            return View(dto);
        }

        // POST: Admin/Country/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateCountryDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Country/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }
    }
}
