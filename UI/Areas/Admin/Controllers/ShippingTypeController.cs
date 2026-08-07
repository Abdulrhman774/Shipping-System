using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class ShippingTypeController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<ShippingTypeRowItem>
        {
            PageTitle = "Shipping Type Management",
            PageDescription = "Manage shipping types...",
            AddButtonText = "Add Shipping Type",
            AddButtonController = "ShippingType",
            RecordLabel = "records",
            RecordIcon = "fa-box",
            SortedBy = "English Name",
            Items = new List<ShippingTypeRowItem>
            {
                new() { Id = Guid.NewGuid(), EnglishName = "Standard", ArabicName = "عادي", ShippingFactor = "1.0x", Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), EnglishName = "Express", ArabicName = "سريع", ShippingFactor = "1.5x", Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), EnglishName = "Same Day", ArabicName = "نفس اليوم", ShippingFactor = "2.0x", Status = enShipmentStatus.Created }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 3, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ShippingTypeFormViewModel());
    }

    [HttpPost]
    public IActionResult Create(ShippingTypeFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new ShippingTypeFormViewModel { Id = id, EnglishName = "Standard", ArabicName = "عادي", ShippingFactor = 1.0, IsActive = true });
    }

    [HttpPost]
    public IActionResult Edit(ShippingTypeFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
