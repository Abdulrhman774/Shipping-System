using BL.Contract.IServices;
using BL.DTOs.PaymentMethod;
using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;
using System.Linq.Expressions;
using Domain.Entities;

namespace UI.Areas.Admin.Controllers;

public class PaymentMethodController : BaseAdminController
{
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    public async Task<IActionResult> Index(string? search = null, string? status = null, int page = 1, int pageSize = 8)
    {
        string? appliedStatus = null;

        if (status == null)
            appliedStatus = "Active";
        else if (status == "All")
            appliedStatus = null;
        else
            appliedStatus = status;

        Expression<Func<TbPaymentMethod, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => (c.MethodEname.Contains(search) || c.MethdAname.Contains(search)) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => (c.MethodEname.Contains(search) || c.MethdAname.Contains(search));
        }
        else if (!string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => c.CurrentState == statusEnum;
        }
        else
        {
            filter = null;
        }

        var result = await _paymentMethodService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<PaymentMethodRowItem>());

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(c => new PaymentMethodRowItem
        {
            Id = c.Id,
            MethodEname = c.MethodEname ?? string.Empty,
            MethdAname = c.MethdAname ?? string.Empty,
            Commission = c.Commission,
            Status = c.CurrentState,
            CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
        }).ToList();

        var model = new ManagementPageViewModel<PaymentMethodRowItem>
        {
            PageTitle = "Payment Method Management",
            PageDescription = "Manage payment methods and operational status.",
            AddButtonText = "Add New Payment Method",
            AddButtonController = "PaymentMethod",
            RecordLabel = "records",
            RecordIcon = "fa-credit-card",
            SortedBy = "Method Name",
            Items = items,
            Pagination = new PaginationModel
            {
                CurrentPage = pagedResult.PageNumber,
                TotalPages = pagedResult.TotalPages,
                TotalItems = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                ItemLabel = "records"
            }
        };

        ViewBag.Search = search;
        ViewBag.Status = appliedStatus ?? "All";

        return View(model);
    }

    [HttpGet]
    public IActionResult Create() => View(new PaymentMethodFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentMethodFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CreatePaymentMethodDto
        {
            MethodEname = model.MethodEname,
            MethdAname = model.MethdAname,
            Commission = model.Commission
        };

        var result = await _paymentMethodService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create payment method.");
            return View(model);
        }

        var createdId = result.Value.Id;
        await _paymentMethodService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "Payment Method created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _paymentMethodService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "Payment Method not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new PaymentMethodFormViewModel
        {
            Id = dto.Id,
            MethodEname = dto.MethodEname,
            MethdAname = dto.MethdAname,
            Commission = dto.Commission,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PaymentMethodFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new UpdatePaymentMethodDto
        {
            MethodEname = model.MethodEname,
            MethdAname = model.MethdAname,
            Commission = model.Commission
        };

        var updateResult = await _paymentMethodService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update payment method.");
            return View(model);
        }

        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _paymentMethodService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "Payment Method updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _paymentMethodService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "Payment Method deleted successfully." : "Failed to delete payment method.";
        return RedirectToAction("Index");
    }
}
