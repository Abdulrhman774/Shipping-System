using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class ShipmentController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ShipmentIndexViewModel
        {
            Shipments = new List<ShipmentRowItem>
    {
        new() { RowNumber = 1, TrackingNumber = "TRK-882910", SenderName = "John Doe", ReceiverName = "Jane Smith", ShippingDate = "2023-10-01", Status = enShipmentStatus.Created },
        new() { RowNumber = 2, TrackingNumber = "TRK-882911", SenderName = "Alice Brown", ReceiverName = "Bob White", ShippingDate = "2023-10-02", Status = enShipmentStatus.Approved },
        new() { RowNumber = 3, TrackingNumber = "TRK-882912", SenderName = "Charlie Green", ReceiverName = "David Black", ShippingDate = "2023-10-03", Status = enShipmentStatus.ReadyForShip },
        new() { RowNumber = 4, TrackingNumber = "TRK-882913", SenderName = "Eve Adams", ReceiverName = "Frank Thomas", ShippingDate = "2023-10-04", Status = enShipmentStatus.Shipped },
        new() { RowNumber = 5, TrackingNumber = "TRK-882914", SenderName = "Grace Lee", ReceiverName = "Henry Ford", ShippingDate = "2023-10-05", Status = enShipmentStatus.Delivered },
        new() { RowNumber = 6, TrackingNumber = "TRK-882915", SenderName = "Ivy King", ReceiverName = "Jack Queen", ShippingDate = "2023-10-06", Status = enShipmentStatus.Created },
        new() { RowNumber = 7, TrackingNumber = "TRK-882916", SenderName = "Karen Prince", ReceiverName = "Leo Duke", ShippingDate = "2023-10-07", Status = enShipmentStatus.Approved },
        new() { RowNumber = 8, TrackingNumber = "TRK-882917", SenderName = "Mia Earl", ReceiverName = "Noah Lord", ShippingDate = "2023-10-08", Status = enShipmentStatus.ReadyForShip },
        new() { RowNumber = 9, TrackingNumber = "TRK-882918", SenderName = "Olivia Knight", ReceiverName = "Paul Squire", ShippingDate = "2023-10-09", Status = enShipmentStatus.Shipped },
        new() { RowNumber = 10, TrackingNumber = "TRK-882919", SenderName = "Quinn Page", ReceiverName = "Rose Noble", ShippingDate = "2023-10-10", Status = enShipmentStatus.Delivered }
    },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 13, TotalItems = 124, PageSize = 10, ItemLabel = "shipments" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ShipmentWizardViewModel { CurrentStep = 1 });
    }

    [HttpPost]
    public IActionResult Create(ShipmentWizardViewModel model)
    {
        return RedirectToAction("Index");
    }

    public IActionResult Details(Guid id)
    {
        return View();
    }

    public IActionResult Edit(Guid id)
    {
        return View();
    }
}
