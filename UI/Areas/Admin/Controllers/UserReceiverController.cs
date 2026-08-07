using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class UserReceiverController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<UserReceiverRowItem>
        {
            PageTitle = "Receiver Management",
            PageDescription = "Manage receivers...",
            AddButtonText = "Add Receiver",
            AddButtonController = "UserReceiver",
            RecordLabel = "records",
            RecordIcon = "fa-user",
            SortedBy = "Name",
            Items = new List<UserReceiverRowItem>
            {
                new() { Id = Guid.NewGuid(), Name = "Alice Brown", ReceiverId = "RCV-2001", Email = "alice@example.com", Phone = "1112223333", City = "Riyadh", IsDefault = true, Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), Name = "Bob White", ReceiverId = "RCV-2002", Email = "bob@example.com", Phone = "4445556666", City = "Jeddah", IsDefault = false, Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), Name = "Charlie Green", ReceiverId = "RCV-2003", Email = "charlie@example.com", Phone = "7778889999", City = "Dubai", IsDefault = true, Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), Name = "David Black", ReceiverId = "RCV-2004", Email = "david@example.com", Phone = "0001112222", City = "Cairo", IsDefault = false, Status = enShipmentStatus.Created },
                new() { Id = Guid.NewGuid(), Name = "Eve Adams", ReceiverId = "RCV-2005", Email = "eve@example.com", Phone = "3334445555", City = "Kuwait City", IsDefault = true, Status = enShipmentStatus.Created }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UserReceiverFormViewModel 
        { 
            Cities = new List<SelectListItem> { new SelectListItem { Text = "Riyadh", Value = Guid.NewGuid().ToString() } } 
        });
    }

    [HttpPost]
    public IActionResult Create(UserReceiverFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new UserReceiverFormViewModel 
        { 
            Id = id, 
            Name = "Alice Brown", 
            Email = "alice@example.com", 
            Phone = "1112223333", 
            IsActive = true,
            Cities = new List<SelectListItem> { new SelectListItem { Text = "Riyadh", Value = Guid.NewGuid().ToString() } } 
        });
    }

    [HttpPost]
    public IActionResult Edit(UserReceiverFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
