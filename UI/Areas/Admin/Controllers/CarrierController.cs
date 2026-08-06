using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class CarrierController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<CarrierRowItem>
        {
            PageTitle = "Carrier Management",
            PageDescription = "Manage carriers...",
            AddButtonText = "Add New Carrier",
            AddButtonController = "Carrier",
            RecordLabel = "records",
            RecordIcon = "fa-truck",
            SortedBy = "Carrier Name",
            Items = new List<CarrierRowItem>
            {
                new() { Id = Guid.NewGuid(), CarrierName = "Swift Logistics Group", Status = "Active", CreatedDate = "2023-11-15" },
                new() { Id = Guid.NewGuid(), CarrierName = "Global Freight Systems", Status = "Active", CreatedDate = "2023-12-02" },
                new() { Id = Guid.NewGuid(), CarrierName = "Oceanic Maritime Inc.", Status = "Inactive", CreatedDate = "2024-01-10" },
                new() { Id = Guid.NewGuid(), CarrierName = "Atlas Distribution Network", Status = "Active", CreatedDate = "2024-01-28" },
                new() { Id = Guid.NewGuid(), CarrierName = "Pinnacle Express Services", Status = "Active", CreatedDate = "2024-02-14" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CarrierFormViewModel());
    }

    [HttpPost]
    public IActionResult Create(CarrierFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new CarrierFormViewModel { Id = id, CarrierName = "Swift Logistics Group", IsActive = true });
    }

    [HttpPost]
    public IActionResult Edit(CarrierFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
