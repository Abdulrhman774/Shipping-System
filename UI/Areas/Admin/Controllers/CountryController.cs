using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class CountryController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<CountryRowItem>
        {
            PageTitle = "Country Management",
            PageDescription = "Manage global destinations...",
            AddButtonText = "Add New Country",
            AddButtonController = "Country",
            RecordLabel = "records",
            RecordIcon = "fa-globe",
            SortedBy = "English Name",
            Items = new List<CountryRowItem>
            {
                new() { Id = Guid.NewGuid(), EnglishName = "Saudi Arabia", ArabicName = "المملكة العربية السعودية", Status = "Active", CreatedDate = "2023-01-10" },
                new() { Id = Guid.NewGuid(), EnglishName = "UAE", ArabicName = "الإمارات العربية المتحدة", Status = "Active", CreatedDate = "2023-01-12" },
                new() { Id = Guid.NewGuid(), EnglishName = "Egypt", ArabicName = "جمهورية مصر العربية", Status = "Active", CreatedDate = "2023-02-05" },
                new() { Id = Guid.NewGuid(), EnglishName = "Kuwait", ArabicName = "الكويت", Status = "Active", CreatedDate = "2023-03-20" },
                new() { Id = Guid.NewGuid(), EnglishName = "Jordan", ArabicName = "الأردن", Status = "Inactive", CreatedDate = "2023-04-15" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 5, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CountryFormViewModel());
    }

    [HttpPost]
    public IActionResult Create(CountryFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new CountryFormViewModel { Id = id, EnglishName = "Saudi Arabia", ArabicName = "المملكة العربية السعودية", IsActive = true });
    }

    [HttpPost]
    public IActionResult Edit(CountryFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
