using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class UserSenderController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<UserSenderRowItem>
        {
            PageTitle = "Sender Management",
            PageDescription = "Manage senders...",
            AddButtonText = "Add Sender",
            AddButtonController = "UserSender",
            RecordLabel = "records",
            RecordIcon = "fa-user",
            SortedBy = "Name",
            Items = new List<UserSenderRowItem>
            {
                new() { Id = Guid.NewGuid(), Name = "John Doe", SenderId = "SND-1001", Email = "john@example.com", Phone = "1234567890", City = "Riyadh", IsDefault = true, Status = "Active" },
                new() { Id = Guid.NewGuid(), Name = "Jane Smith", SenderId = "SND-1002", Email = "jane@example.com", Phone = "0987654321", City = "Jeddah", IsDefault = false, Status = "Active" },
                new() { Id = Guid.NewGuid(), Name = "Mike Johnson", SenderId = "SND-1003", Email = "mike@example.com", Phone = "5551234567", City = "Dubai", IsDefault = true, Status = "Active" },
                new() { Id = Guid.NewGuid(), Name = "Sarah Williams", SenderId = "SND-1004", Email = "sarah@example.com", Phone = "4449876543", City = "Cairo", IsDefault = false, Status = "Inactive" },
                new() { Id = Guid.NewGuid(), Name = "Tom Brown", SenderId = "SND-1005", Email = "tom@example.com", Phone = "2223334444", City = "Kuwait City", IsDefault = true, Status = "Active" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UserSenderFormViewModel 
        { 
            Cities = new List<SelectListItem> { new SelectListItem { Text = "Riyadh", Value = Guid.NewGuid().ToString() } } 
        });
    }

    [HttpPost]
    public IActionResult Create(UserSenderFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new UserSenderFormViewModel 
        { 
            Id = id, 
            Name = "John Doe", 
            Email = "john@example.com", 
            Phone = "1234567890", 
            IsActive = true,
            Cities = new List<SelectListItem> { new SelectListItem { Text = "Riyadh", Value = Guid.NewGuid().ToString() } } 
        });
    }

    [HttpPost]
    public IActionResult Edit(UserSenderFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
