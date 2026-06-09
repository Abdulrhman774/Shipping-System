using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.SubscriptionPackage;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SubscriptionPackageController : Controller
    {
        // Constructor inject ISubscriptionPackageService (commented out as per instructions)
        /*
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        public SubscriptionPackageController(ISubscriptionPackageService subscriptionPackageService)
        {
            _subscriptionPackageService = subscriptionPackageService;
        }
        */

        // GET: Admin/SubscriptionPackage
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample subscription packages for display
            var packages = new List<TbSubscriptionPackage>
            {
                new TbSubscriptionPackage
                {
                    Id = Guid.Parse("11111111-7777-7777-7777-111111111111"),
                    PackageName = "الباقة الأساسية (Basic)",
                    ShippimentCount = 100,
                    NumberOfKiloMeters = 500.0,
                    TotalWeight = 1000.0,
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 20, 10, 0, 0)
                },
                new TbSubscriptionPackage
                {
                    Id = Guid.Parse("22222222-8888-8888-8888-222222222222"),
                    PackageName = "الباقة المتقدمة (Premium)",
                    ShippimentCount = 500,
                    NumberOfKiloMeters = 3000.0,
                    TotalWeight = 5000.0,
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 18, 14, 30, 0)
                },
                new TbSubscriptionPackage
                {
                    Id = Guid.Parse("33333333-9999-9999-9999-333333333333"),
                    PackageName = "الباقة غير المحدودة (Enterprise)",
                    ShippimentCount = 2000,
                    NumberOfKiloMeters = 15000.0,
                    TotalWeight = 25000.0,
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 3, 25, 9, 15, 0)
                }
            };

            return View(packages);
        }

        // GET: Admin/SubscriptionPackage/Create
        public IActionResult Create()
        {
            return View(new CreateSubscriptionPackageDto());
        }

        // POST: Admin/SubscriptionPackage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateSubscriptionPackageDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/SubscriptionPackage/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            // Hardcoded update model with pre-filled values
            var dto = new UpdateSubscriptionPackageDto
            {
                PackageName = "الباقة المتقدمة (Premium)",
                ShippimentCount = 500,
                NumberOfKiloMeters = 3000.0,
                TotalWeight = 5000.0
            };
            return View(dto);
        }

        // POST: Admin/SubscriptionPackage/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateSubscriptionPackageDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/SubscriptionPackage/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }
    }
}
