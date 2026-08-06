using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class UserSubscriptionController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<UserSubscriptionRowItem>
        {
            PageTitle = "User Subscriptions",
            PageDescription = "Manage subscriptions...",
            AddButtonText = "Add Subscription",
            AddButtonController = "UserSubscription",
            RecordLabel = "records",
            RecordIcon = "fa-id-card",
            SortedBy = "UserName",
            Items = new List<UserSubscriptionRowItem>
            {
                new() { Id = Guid.NewGuid(), UserName = "John Doe", SubId = "SUB-001", PackageName = "Basic", PackageColor = "blue", SubscribedDate = "Jan 10, 2026", UsedShipments = 10, TotalShipments = 100, ExpiryDate = "Feb 10, 2026", Status = "Active" },
                new() { Id = Guid.NewGuid(), UserName = "Jane Smith", SubId = "SUB-002", PackageName = "Pro", PackageColor = "green", SubscribedDate = "Dec 01, 2025", UsedShipments = 50, TotalShipments = 500, ExpiryDate = "Dec 01, 2026", Status = "Active" },
                new() { Id = Guid.NewGuid(), UserName = "Mike Johnson", SubId = "SUB-003", PackageName = "Enterprise", PackageColor = "purple", SubscribedDate = "Jun 15, 2025", UsedShipments = 1200, TotalShipments = 2000, ExpiryDate = "Jun 15, 2026", Status = "Active" },
                new() { Id = Guid.NewGuid(), UserName = "Sarah Williams", SubId = "SUB-004", PackageName = "Basic", PackageColor = "blue", SubscribedDate = "Jan 20, 2025", UsedShipments = 100, TotalShipments = 100, ExpiryDate = "Feb 20, 2026", Status = "Expired" },
                new() { Id = Guid.NewGuid(), UserName = "Tom Brown", SubId = "SUB-005", PackageName = "Pro", PackageColor = "green", SubscribedDate = "Jul 25, 2026", UsedShipments = 2, TotalShipments = 500, ExpiryDate = "Oct 25, 2026", Status = "Active" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UserSubscriptionFormViewModel 
        { 
            Users = new List<SelectListItem> { new SelectListItem { Text = "John Doe", Value = "user-1" } },
            Packages = new List<SelectListItem> { new SelectListItem { Text = "Basic", Value = Guid.NewGuid().ToString() } }
        });
    }

    [HttpPost]
    public IActionResult Create(UserSubscriptionFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new UserSubscriptionFormViewModel 
        { 
            Id = id, 
            IsActive = true,
            Users = new List<SelectListItem> { new SelectListItem { Text = "John Doe", Value = "user-1" } },
            Packages = new List<SelectListItem> { new SelectListItem { Text = "Basic", Value = Guid.NewGuid().ToString() } }
        });
    }

    [HttpPost]
    public IActionResult Edit(UserSubscriptionFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
