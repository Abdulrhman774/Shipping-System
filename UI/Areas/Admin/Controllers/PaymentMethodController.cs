using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class PaymentMethodController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new ManagementPageViewModel<PaymentMethodRowItem>
        {
            PageTitle = "Payment Method Management",
            PageDescription = "Manage payment methods...",
            AddButtonText = "Add Payment Method",
            AddButtonController = "PaymentMethod",
            RecordLabel = "records",
            RecordIcon = "fa-credit-card",
            SortedBy = "English Name",
            Items = new List<PaymentMethodRowItem>
            {
                new() { Id = Guid.NewGuid(), EnglishName = "Credit Card", ArabicName = "بطاقة ائتمان", Commission = "2.0%", Status = "Active", Icon = "fa-cc-visa" },
                new() { Id = Guid.NewGuid(), EnglishName = "Cash on Delivery", ArabicName = "الدفع عند الاستلام", Commission = "5.0%", Status = "Active", Icon = "fa-money-bill" },
                new() { Id = Guid.NewGuid(), EnglishName = "PayPal", ArabicName = "باي بال", Commission = "3.0%", Status = "Inactive", Icon = "fa-paypal" }
            },
            Pagination = new PaginationModel { CurrentPage = 1, TotalPages = 1, TotalItems = 3, PageSize = 10, ItemLabel = "records" }
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new PaymentMethodFormViewModel());
    }

    [HttpPost]
    public IActionResult Create(PaymentMethodFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new PaymentMethodFormViewModel { Id = id, EnglishName = "Credit Card", ArabicName = "بطاقة ائتمان", Commission = 2.0, IsActive = true });
    }

    [HttpPost]
    public IActionResult Edit(PaymentMethodFormViewModel model)
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        return RedirectToAction("Index");
    }
}
