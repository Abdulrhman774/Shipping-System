using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.UserSubscription;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserSubscriptionController : BaseController
    {
        // Constructor inject IUserSubscriptionService (commented out as per instructions)
        /*
        private readonly IUserSubscriptionService _userSubscriptionService;
        public UserSubscriptionController(IUserSubscriptionService userSubscriptionService)
        {
            _userSubscriptionService = userSubscriptionService;
        }
        */

        // GET: Admin/UserSubscription
        public IActionResult Index()
        {
            // Hardcoded list of user subscriptions for display
            var subscriptions = new List<TbUserSubscription>
            {
                new TbUserSubscription
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    UserId = Guid.Parse("aaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                    // Using a custom property or placeholder for User representation in view
                    SubscriptionDate = new DateTime(2026, 5, 1, 9, 0, 0),
                    Package = new TbSubscriptionPackage { PackageName = "Premium Gold" },
                    CurrentState = enEntityState.Active
                },
                new TbUserSubscription
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    UserId = Guid.Parse("fffffff-eeee-dddd-cccc-bbbbbbbbbbbb"),
                    SubscriptionDate = new DateTime(2026, 6, 2, 14, 30, 0),
                    Package = new TbSubscriptionPackage { PackageName = "Basic Plan" },
                    CurrentState = enEntityState.Active
                }
            };

            return View(subscriptions);
        }

        // GET: Admin/UserSubscription/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new CreateUserSubscriptionDto { SubscriptionDate = DateTime.UtcNow });
        }

        // POST: Admin/UserSubscription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserSubscriptionDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/UserSubscription/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            // Loading sample packages
            var packages = new List<TbSubscriptionPackage>
            {
                new TbSubscriptionPackage { Id = Guid.Parse("b1111111-2222-3333-4444-555555555555"), PackageName = "Premium Gold" },
                new TbSubscriptionPackage { Id = Guid.Parse("b2222222-3333-4444-5555-666666666666"), PackageName = "Basic Plan" }
            };
            ViewBag.Packages = new SelectList(packages, "Id", "PackageName");

            // Loading sample users (senders as subscribers for representation)
            var users = new List<TbUserSender>
            {
                new TbUserSender { Id = Guid.Parse("b1111111-2222-3333-4444-555555555555"), SenderName = "أحمد محمد (Ahmed Mohamed)" },
                new TbUserSender { Id = Guid.Parse("b2222222-3333-4444-5555-666666666666"), SenderName = "خالد عبدالله (Khalid Abdullah)" }
            };
            ViewBag.Users = new SelectList(users, "Id", "SenderName");
        }
    }
}
