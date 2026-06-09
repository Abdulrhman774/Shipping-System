using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.City;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CityController : BaseController
    {
        // Constructor inject ICityService (commented out as per instructions)
        /*
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }
        */

        // GET: Admin/City
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample cities for display
            var cities = new List<TbCity>
            {
                new TbCity
                {
                    Id = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                    CityAname = "القاهرة",
                    CityEname = "Cairo",
                    CountryId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"),
                    Country = new TbCountry { CountryAname = "مصر", CountryEname = "Egypt" },
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 2, 11, 0, 0)
                },
                new TbCity
                {
                    Id = Guid.Parse("e2f3a4b5-c6d7-8e9f-0a1b-2c3d4e5f6a7b"),
                    CityAname = "الرياض",
                    CityEname = "Riyadh",
                    CountryId = Guid.Parse("b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e"),
                    Country = new TbCountry { CountryAname = "المملكة العربية السعودية", CountryEname = "Saudi Arabia" },
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 16, 15, 0, 0)
                },
                new TbCity
                {
                    Id = Guid.Parse("d3e4f5a6-b7c8-9d0e-1f2a-3b4c5d6e7f8a"),
                    CityAname = "نيويورك",
                    CityEname = "New York",
                    CountryId = Guid.Parse("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                    Country = new TbCountry { CountryAname = "الولايات المتحدة", CountryEname = "United States" },
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 3, 11, 10, 15, 0)
                }
            };

            return View(cities);
        }

        // GET: Admin/City/Create
        public IActionResult Create()
        {
            LoadCountries();
            return View(new CreateCityDto());
        }

        // POST: Admin/City/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCityDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/City/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            LoadCountries();
            // Hardcoded update model with pre-filled values
            var dto = new UpdateCityDto
            {
                CityAname = "القاهرة",
                CityEname = "Cairo",
                CountryId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d")
            };
            return View(dto);
        }

        // POST: Admin/City/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateCityDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/City/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        private void LoadCountries()
        {
            var countries = new List<TbCountry>
            {
                new TbCountry { Id = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"), CountryAname = "مصر", CountryEname = "Egypt" },
                new TbCountry { Id = Guid.Parse("b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e"), CountryAname = "المملكة العربية السعودية", CountryEname = "Saudi Arabia" },
                new TbCountry { Id = Guid.Parse("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), CountryAname = "الولايات المتحدة", CountryEname = "United States" }
            };
            ViewBag.Countries = new SelectList(countries, "Id", "CountryEname");
        }
    }
}
