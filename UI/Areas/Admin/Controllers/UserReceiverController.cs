using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.UserReceiver;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserReceiverController : BaseController
    {
        // Constructor inject IUserReceiverService (commented out as per instructions)
        /*
        private readonly IUserReceiverService _userReceiverService;
        public UserReceiverController(IUserReceiverService userReceiverService)
        {
            _userReceiverService = userReceiverService;
        }
        */

        // GET: Admin/UserReceiver
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample receivers for display
            var receivers = new List<TbUserReceiver>
            {
                new TbUserReceiver
                {
                    Id = Guid.Parse("44444444-1111-1111-1111-444444444444"),
                    UserId = Guid.Parse("dd44ee55-66ff-77aa-88bb-99cc00dd11ee"),
                    ReceiverName = "سارة أحمد (Sarah Ahmed)",
                    Email = "sarah.ahmed@example.com",
                    Phone = "+201009876543",
                    CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                    City = new TbCity { CityAname = "القاهرة", CityEname = "Cairo" },
                    Address = "شارع التحرير، الدقي، القاهرة",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 15, 10, 0, 0)
                },
                new TbUserReceiver
                {
                    Id = Guid.Parse("55555555-2222-2222-2222-555555555555"),
                    UserId = Guid.Parse("ee55ff66-77aa-88bb-99cc-00dd11ee22ff"),
                    ReceiverName = "علي حسن (Ali Hassan)",
                    Email = "ali.hassan@example.com",
                    Phone = "+966509876543",
                    CityId = Guid.Parse("e2f3a4b5-c6d7-8e9f-0a1b-2c3d4e5f6a7b"),
                    City = new TbCity { CityAname = "الرياض", CityEname = "Riyadh" },
                    Address = "حي الملقا، الرياض",
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 20, 14, 30, 0)
                },
                new TbUserReceiver
                {
                    Id = Guid.Parse("66666666-3333-3333-3333-666666666666"),
                    UserId = Guid.Parse("ff66aa77-88bb-99cc-00dd-11ee22ff33aa"),
                    ReceiverName = "فاطمة عمر (Fatima Omar)",
                    Email = "fatima.omar@example.com",
                    Phone = "+12025550199",
                    CityId = Guid.Parse("d3e4f5a6-b7c8-9d0e-1f2a-3b4c5d6e7f8a"),
                    City = new TbCity { CityAname = "نيويورك", CityEname = "New York" },
                    Address = "Broadway, New York, NY",
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 4, 5, 16, 45, 0)
                }
            };

            return View(receivers);
        }

        // GET: Admin/UserReceiver/Details/{id}
        public IActionResult Details(Guid id)
        {
            // Hardcoded receiver details for display
            var receiver = new TbUserReceiver
            {
                Id = id,
                UserId = Guid.Parse("dd44ee55-66ff-77aa-88bb-99cc00dd11ee"),
                ReceiverName = "سارة أحمد (Sarah Ahmed)",
                Email = "sarah.ahmed@example.com",
                Phone = "+201009876543",
                CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                City = new TbCity { CityAname = "القاهرة", CityEname = "Cairo" },
                Address = "شارع التحرير، الدقي، القاهرة",
                CurrentState = enEntityState.Active,
                CreatedDate = new DateTime(2026, 1, 15, 10, 0, 0)
            };

            // Hardcoded list of shipments received by this receiver
            var shipments = new List<dynamic>
            {
                new { Code = "SHP-00109", Sender = "أحمد محمد", Date = new DateTime(2026, 5, 2, 10, 15, 0), Type = "Express", Weight = 3.5, Status = "Delivered" },
                new { Code = "SHP-00220", Sender = "خالد عبدالله", Date = new DateTime(2026, 5, 20, 11, 0, 0), Type = "Standard", Weight = 5.0, Status = "In Transit" }
            };

            ViewBag.Shipments = shipments;
            return View(receiver);
        }

        // GET: Admin/UserReceiver/Create
        public IActionResult Create()
        {
            LoadCities();
            return View(new CreateUserReceiverDto());
        }

        // POST: Admin/UserReceiver/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserReceiverDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/UserReceiver/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            LoadCities();
            // Hardcoded update model with pre-filled values
            var dto = new UpdateUserReceiverDto
            {
                UserId = Guid.Parse("dd44ee55-66ff-77aa-88bb-99cc00dd11ee"),
                ReceiverName = "سارة أحمد (Sarah Ahmed)",
                Email = "sarah.ahmed@example.com",
                Phone = "+201009876543",
                CityId = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c"),
                Address = "شارع التحرير، الدقي، القاهرة"
            };
            return View(dto);
        }

        // POST: Admin/UserReceiver/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdateUserReceiverDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/UserReceiver/Delete/{id}
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
