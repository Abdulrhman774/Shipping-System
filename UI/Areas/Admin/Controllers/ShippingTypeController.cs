using BL.Contract.IServices;
using BL.DTOs.ShippingType;
using BL.Services;
using BL.Services.Simulation;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ShippingTypeController : BaseController
    {
        private readonly IShippingTypeService _service;
        private readonly ILogger<ShippingTypeService> _logger;
        public ShippingTypeController(IShippingTypeService service, ILogger<ShippingTypeService> logger)
        {
            _service = service;
            _logger = logger;     
        }

        // Constructor inject IShippingTypeService (commented out as per instructions)
        /*
        private readonly IShippingTypeService _shippingTypeService;
        public ShippingTypeController(IShippingTypeService shippingTypeService)
        { m
            _shippingTypeService = shippingTypeService;
        }
        */

        // GET: Admin/ShippingType
        public async Task<IActionResult> Index()
        {
            var shippingTypes = await _service.GetAllAsync();
            return View(shippingTypes);
        }

        // GET: Admin/ShippingType/Create
        public IActionResult Create()
        {
            return View(new CreateShippingTypeDto());
        }

        // POST: Admin/ShippingType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateShippingTypeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _service.AddAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to create shipping type");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var shippingType = await _service.GetByIdAsync(id);

            // Store the ID in ViewBag to use it in the view
            ViewBag.Id = id;

            if (shippingType == null)
                return NotFound();

            var dto = new UpdateShippingTypeDto
            {
                ShippingTypeAname = shippingType.ShippingTypeAname,
                ShippingTypeEname = shippingType.ShippingTypeEname,
                ShippingFactor = shippingType.ShippingFactor
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Guid id, UpdateShippingTypeDto dto)
        {
            if (!ModelState.IsValid)
                return View("Edit", dto);

            if (id == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid shipping type ID");
                return View("Edit", dto);
            }

            try
            {
                var result = await _service.UpdateAsync(id, dto);

                if (!result)
                {
                    TempData["MessageType"] = enMessageType.SavedFailed;
                    return View("Edit", dto);
                }

                TempData["MessageType"] = enMessageType.SavedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating shipping type {Id}", id);

                TempData["MessageType"] = enMessageType.SavedFailed;
                return View("Edit", dto);
            }
        }

        // POST: Admin/ShippingType/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
