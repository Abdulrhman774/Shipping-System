using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class SubscriptionPackageController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<SubscriptionPackageRowItem>
        {
            PageTitle = "Subscription Package Management",
            PageDescription = "Manage subscription packages...",
            AddButtonText = "Add Package",
            AddButtonController = "SubscriptionPackage",
            RecordLabel = "records",
            RecordIcon = "fa-box-open",
            SortedBy = "Package Name",
            Items = new List<SubscriptionPackageRowItem>
            {
                new() { Id = Guid.NewGuid(), PackageName = "Basic", Uid = "PKG-001", ShipmentCount = "100", DistanceKm = "1,000", WeightKg = "500", Price = "$99.99", DurationDays = "30", Status = enShipmentStatus.Created, Icon = "fa-star" },
                new() { Id = Guid.NewGuid(), PackageName = "Pro", Uid = "PKG-002", ShipmentCount = "500", DistanceKm = "5,000", WeightKg = "2,000", Price = "$299.99", DurationDays = "90", Status = enShipmentStatus.Created, Icon = "fa-medal" },
                new() { Id = Guid.NewGuid(), PackageName = "Enterprise", Uid = "PKG-003", ShipmentCount = "2,000", DistanceKm = "20,000", WeightKg = "10,000", Price = "$999.99", DurationDays = "365", Status = enShipmentStatus.Created, Icon = "fa-crown" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 3, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SubscriptionPackageFormViewModel());
    }

    [HttpPost]
    public IActionResult Create(SubscriptionPackageFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new SubscriptionPackageFormViewModel { Id = id, PackageName = "Basic", ShipmentCount = 100, NumberOfKiloMeters = 1000, TotalWeight = 500, Price = 99.99m, DurationDays = 30, IsActive = true });
    }

    [HttpPost]
    public IActionResult Edit(SubscriptionPackageFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
