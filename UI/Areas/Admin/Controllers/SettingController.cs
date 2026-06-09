using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Domain.Entities;
using BL.DTOs.Setting;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingController : Controller
    {
        // Constructor inject ISettingService (commented out as per instructions)
        /*
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        */

        // GET: Admin/Setting/Edit
        public IActionResult Edit()
        {
            // Hardcoded setting model with pre-filled values
            var dto = new UpdateSettingDto
            {
                KiloMeterRate = 5.50,
                KilooGramRate = 2.25
            };
            return View(dto);
        }

        // POST: Admin/Setting/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateSettingDto dto)
        {
            // Redirect back to Edit page (no business logic)
            return RedirectToAction(nameof(Edit));
        }
    }
}
