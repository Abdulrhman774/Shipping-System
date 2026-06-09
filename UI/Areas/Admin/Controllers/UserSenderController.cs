using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.UserSender;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserSenderController : Controller
    {
        // Constructor inject IUserSenderService (commented out as per instructions)
        /*
        private readonly IUserSenderService _userSenderService;
        public UserSenderController(IUserSenderService userSenderService)
        {
            _userSenderService = userSenderService;
        }
        */

        // GET: Admin/UserSender
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample senders for display
            var senders = new List<TbUserSender>
            {
                new TbUserSender
                {
                    Id = Guid.Parse("11111111-eeee-eeee-eeee-111111111111"),
                    UserId = Guid.Parse("aa11bb22-33cc-44dd-55ee-66ff77aa88bb"),
                    SenderName = "أحمد محمد (Ahmed Mohamed)",
                    Email = "ahmed.mohamed@example.com",
                    Phone = "+201001234567",
                    CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                    City = new TbCity { CityAname = "القاهرة", CityEname = "Cairo" },
                    Address = "شارع المعز، القاهرة القديمة",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 10, 9, 0, 0)
                },
                new TbUserSender
                {
                    Id = Guid.Parse("22222222-ffff-ffff-ffff-222222222222"),
                    UserId = Guid.Parse("bb22cc33-44dd-55ee-66ff-77aa88bb99cc"),
                    SenderName = "خالد عبدالله (Khalid Abdullah)",
                    Email = "khalid.a@example.com",
                    Phone = "+966501234567",
                    CityId = Guid.Parse("e2f3a4b5-c6d7-8e9f-0a1b-2c3d4e5f6a7b"),
                    City = new TbCity { CityAname = "الرياض", CityEname = "Riyadh" },
                    Address = "شارع العليا، الرياض",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 5, 11, 30, 0)
                },
                new TbUserSender
                {
                    Id = Guid.Parse("33333333-0000-0000-0000-333333333333"),
                    UserId = Guid.Parse("cc33dd44-55ee-66ff-77aa-88bb99cc00dd"),
                    SenderName = "جون دو (John Doe)",
                    Email = "john.doe@example.com",
                    Phone = "+12025550143",
                    CityId = Guid.Parse("d3e4f5a6-b7c8-9d0e-1f2a-3b4c5d6e7f8a"),
                    City = new TbCity { CityAname = "نيويورك", CityEname = "New York" },
                    Address = "5th Avenue, New York, NY",
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 3, 15, 15, 45, 0)
                }
            };

            return View(senders);
        }

        // GET: Admin/UserSender/Details/{id}
        public IActionResult Details(Guid id)
        {
            // Hardcoded sender details for display
            var sender = new TbUserSender
            {
                Id = id,
                UserId = Guid.Parse("aa11bb22-33cc-44dd-55ee-66ff77aa88bb"),
                SenderName = "أحمد محمد (Ahmed Mohamed)",
                Email = "ahmed.mohamed@example.com",
                Phone = "+201001234567",
                CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                City = new TbCity { CityAname = "القاهرة", CityEname = "Cairo" },
                Address = "شارع المعز، القاهرة القديمة",
                CurrentState = enEntityState.Active,
                CreatedDate = new DateTime(2026, 1, 10, 9, 0, 0)
            };

            // Hardcoded list of shipments sent by this sender
            var shipments = new List<dynamic>
            {
                new { Code = "SHP-00109", Receiver = "سارة أحمد", Date = new DateTime(2026, 5, 2, 10, 15, 0), Type = "Express", Weight = 3.5, Status = "Delivered" },
                new { Code = "SHP-00214", Receiver = "علي حسن", Date = new DateTime(2026, 5, 18, 14, 0, 0), Type = "Standard", Weight = 12.0, Status = "In Transit" },
                new { Code = "SHP-00305", Receiver = "فاطمة عمر", Date = new DateTime(2026, 6, 1, 9, 30, 0), Type = "Standard", Weight = 1.2, Status = "Pending" }
            };

            ViewBag.Shipments = shipments;
            return View(sender);
        }

        // GET: Admin/UserSender/Create
        public IActionResult Create()
        {
            LoadCities();
            return View(new CreateUserSenderDto());
        }

        // POST: Admin/UserSender/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserSenderDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/UserSender/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            LoadCities();
            // Hardcoded update model with pre-filled values
            var dto = new UpdateUserSenderDto
            {
                UserId = Guid.Parse("aa11bb22-33cc-44dd-55ee-66ff77aa88bb"),
                SenderName = "أحمد محمد (Ahmed Mohamed)",
                Email = "ahmed.mohamed@example.com",
                Phone = "+201001234567",
                CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                Address = "شارع المعز، القاهرة القديمة"
            };
            return View(dto);
        }

        // POST: Admin/UserSender/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateUserSenderDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/UserSender/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        private void LoadCities()
        {
            var cities = new List<TbCity>
            {
                new TbCity { Id = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"), CityAname = "القاهرة", CityEname = "Cairo" },
                new TbCity { Id = Guid.Parse("e2f3a4b5-c6d7-8e9f-0a1b-2c3d4e5f6a7b"), CityAname = "الرياض", CityEname = "Riyadh" },
                new TbCity { Id = Guid.Parse("d3e4f5a6-b7c8-9d0e-1f2a-3b4c5d6e7f8a"), CityAname = "نيويورك", CityEname = "New York" }
            };
            ViewBag.Cities = new SelectList(cities, "Id", "CityEname");
        }
    }
}
