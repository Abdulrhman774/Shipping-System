using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.Carrier;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CarrierController : BaseController
    {
        // Constructor inject ICarrierService (commented out as per instructions)
        /*
        private readonly ICarrierService _carrierService;
        public CarrierController(ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }
        */

        // GET: Admin/Carrier
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample carriers for display
            var carriers = new List<TbCarrier>
            {
                new TbCarrier
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    CarrierName = "DHL Express",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 5, 8, 0, 0)
                },
                new TbCarrier
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    CarrierName = "Aramex",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 12, 10, 30, 0)
                },
                new TbCarrier
                {
                    Id = Guid.Parse("33333333-4444-5555-6666-777777777777"),
                    CarrierName = "FedEx",
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 3, 20, 14, 15, 0)
                }
            };

            return View(carriers);
        }

        // GET: Admin/Carrier/Create
        public IActionResult Create()
        {
            return View(new CreateCarrierDto());
        }

        // POST: Admin/Carrier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCarrierDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Carrier/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            // Hardcoded update model with pre-filled values
            var dto = new UpdateCarrierDto
            {
                CarrierName = "DHL Express"
            };
            return View(dto);
        }

        // POST: Admin/Carrier/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateCarrierDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Carrier/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }
    }
}
