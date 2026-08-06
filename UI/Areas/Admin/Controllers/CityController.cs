using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class CityController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<CityRowItem>
        {
            PageTitle = "City Management",
            PageDescription = "Manage cities...",
            AddButtonText = "Add New City",
            AddButtonController = "City",
            RecordLabel = "records",
            RecordIcon = "fa-city",
            SortedBy = "English Name",
            Items = new List<CityRowItem>
            {
                new() { Id = Guid.NewGuid(), EnglishName = "Riyadh", ArabicName = "الرياض", CountryName = "Saudi Arabia", Status = "Active", CreatedDate = "2023-01-15" },
                new() { Id = Guid.NewGuid(), EnglishName = "Jeddah", ArabicName = "جدة", CountryName = "Saudi Arabia", Status = "Active", CreatedDate = "2023-01-15" },
                new() { Id = Guid.NewGuid(), EnglishName = "Dubai", ArabicName = "دبي", CountryName = "UAE", Status = "Active", CreatedDate = "2023-01-20" },
                new() { Id = Guid.NewGuid(), EnglishName = "Cairo", ArabicName = "القاهرة", CountryName = "Egypt", Status = "Active", CreatedDate = "2023-02-10" },
                new() { Id = Guid.NewGuid(), EnglishName = "Kuwait City", ArabicName = "مدينة الكويت", CountryName = "Kuwait", Status = "Active", CreatedDate = "2023-03-25" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CityFormViewModel 
        { 
            Countries = new List<SelectListItem> 
            { 
                new SelectListItem { Text = "Saudi Arabia", Value = Guid.NewGuid().ToString() },
                new SelectListItem { Text = "UAE", Value = Guid.NewGuid().ToString() }
            } 
        });
    }

    [HttpPost]
    public IActionResult Create(CityFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new CityFormViewModel 
        { 
            Id = id, 
            EnglishName = "Riyadh", 
            ArabicName = "الرياض", 
            IsActive = true,
            Countries = new List<SelectListItem> 
            { 
                new SelectListItem { Text = "Saudi Arabia", Value = Guid.NewGuid().ToString() }
            } 
        });
    }

    [HttpPost]
    public IActionResult Edit(CityFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
