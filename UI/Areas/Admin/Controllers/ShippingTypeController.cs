using BL.Contract.IServices;
using BL.DTOs.ShippingType;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.OpManager + "," + AppRoles.Op)]
public class ShippingTypeController : BaseAdminController
{
    private readonly IShippingTypeService _shippingTypeService;

    public ShippingTypeController(IShippingTypeService shippingTypeService)
    {
        _shippingTypeService = shippingTypeService;
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

        Expression<Func<TbShippingType, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => (c.ShippingTypeEname.Contains(search) || c.ShippingTypeAname.Contains(search)) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => c.ShippingTypeEname.Contains(search) || c.ShippingTypeAname.Contains(search);
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

        var result = await _shippingTypeService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<ShippingTypeRowItem>());

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(c => new ShippingTypeRowItem
        {
            Id = c.Id,
            ShippingTypeEname = c.ShippingTypeEname ?? string.Empty,
            ShippingTypeAname = c.ShippingTypeAname ?? string.Empty,
            ShippingFactor = c.ShippingFactor,
            Status = c.CurrentState,
            CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
        }).ToList();

        var model = new ManagementPageViewModel<ShippingTypeRowItem>
        {
            PageTitle = "Shipping Type Management",
            PageDescription = "Manage shipping types and factors for regional logistics routing.",
            AddButtonText = "Add New Shipping Type",
            AddButtonController = "ShippingType",
            RecordLabel = "records",
            RecordIcon = "fa-tags",
            SortedBy = "Shipping Type Name",
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
    public IActionResult Create() => View(new ShippingTypeFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShippingTypeFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CreateShippingTypeDto
        {
            ShippingTypeEname = model.ShippingTypeEname,
            ShippingTypeAname = model.ShippingTypeAname,
            ShippingFactor = model.ShippingFactor
        };

        var result = await _shippingTypeService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create shipping type.");
            return View(model);
        }

        var createdId = result.Value.Id;
        await _shippingTypeService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "Shipping type created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _shippingTypeService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "Shipping type not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new ShippingTypeFormViewModel
        {
            Id = dto.Id,
            ShippingTypeEname = dto.ShippingTypeEname ?? string.Empty,
            ShippingTypeAname = dto.ShippingTypeAname ?? string.Empty,
            ShippingFactor = dto.ShippingFactor,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ShippingTypeFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new UpdateShippingTypeDto
        {
            ShippingTypeEname = model.ShippingTypeEname,
            ShippingTypeAname = model.ShippingTypeAname,
            ShippingFactor = model.ShippingFactor
        };

        var updateResult = await _shippingTypeService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update shipping type.");
            return View(model);
        }

        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _shippingTypeService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "Shipping type updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _shippingTypeService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "Shipping type deleted successfully." : "Failed to delete shipping type.";
        return RedirectToAction("Index");
    }
}
