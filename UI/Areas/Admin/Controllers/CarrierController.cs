using BL.Contract.IServices;
using BL.DTOs.Carrier;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

public class CarrierController : BaseAdminController
{
    private readonly ICarrierService _carrierService;

    public CarrierController(ICarrierService carrierService)
    {
        _carrierService = carrierService;
    }

    public async Task<IActionResult> Index(string? search = null, string? status = null, int page = 1, int pageSize = 8)
    {
        // 1. تحديد الحالة المطبقة (appliedStatus) بشكل صحيح
        string? appliedStatus = null;

        if (status == null)           // أول تحميل → Active افتراضي
            appliedStatus = "Active";
        else if (status == "All")     // اختار All Statuses → بدون فلتر
            appliedStatus = null;
        else                          // Active أو Inactive صراحةً
            appliedStatus = status;

        // 2. بناء Expression<Func<TbCarrier, bool>> للفلتر
        Expression<Func<TbCarrier, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => c.CarrierName.Contains(search) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => c.CarrierName.Contains(search);
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

        // 3. استدعاء الخدمة مع الفلتر
        var result = await _carrierService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<CarrierRowItem>());

        var pagedResult = result.Value;

        // 4. تحويل البيانات إلى ViewModel
        var items = pagedResult.Items.Select(c => new CarrierRowItem
        {
            Id = c.Id,
            CarrierName = c.CarrierName,
            Status = c.CurrentState,
            CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
        }).ToList();

        var model = new ManagementPageViewModel<CarrierRowItem>
        {
            PageTitle = "Carrier Management",
            PageDescription = "Manage global destinations and operational status for regional logistics routing.",
            AddButtonText = "Add New Carrier",
            AddButtonController = "Carrier",
            RecordLabel = "records",
            RecordIcon = "fa-truck",
            SortedBy = "Carrier Name",
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
        ViewBag.Status = appliedStatus ?? "All"; // نمرر "" إذا كان null لكي يظهر "All Statuses" في القائمة

        return View(model);
    }
    [HttpGet]
    public IActionResult Create() => View(new CarrierFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarrierFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CreateCarrierDto
        {
            CarrierName = model.CarrierName
        };

        var result = await _carrierService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create carrier.");
            return View(model);
        }

        // ✅ تحديث الحالة بعد الإنشاء (لأن الـ CreateCarrierDto لا يحتوي على الحالة)
        var createdId = result.Value.Id;
        await _carrierService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "Carrier created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _carrierService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "Carrier not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new CarrierFormViewModel
        {
            Id = dto.Id,
            CarrierName = dto.CarrierName,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CarrierFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new UpdateCarrierDto
        {
            CarrierName = model.CarrierName
        };

        var updateResult = await _carrierService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update carrier.");
            return View(model);
        }

        // ✅ تحديث الحالة
        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _carrierService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "Carrier updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _carrierService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "Carrier deleted successfully." : "Failed to delete carrier.";
        return RedirectToAction("Index");
    }
}