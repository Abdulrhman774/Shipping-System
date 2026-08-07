using BL.Contract.IServices;
using BL.DTOs.SubscriptionPackage;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.OpManager + "," + AppRoles.Op)]
public class SubscriptionPackageController : BaseAdminController
{
    private readonly ISubscriptionPackageService _subscriptionPackageService;

    public SubscriptionPackageController(ISubscriptionPackageService subscriptionPackageService)
    {
        _subscriptionPackageService = subscriptionPackageService;
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

        Expression<Func<TbSubscriptionPackage, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => c.PackageName.Contains(search) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => c.PackageName.Contains(search);
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

        var result = await _subscriptionPackageService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<SubscriptionPackageRowItem>());

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(c => new SubscriptionPackageRowItem
        {
            Id = c.Id,
            PackageName = c.PackageName ?? string.Empty,
            ShipimentCount = c.ShipimentCount,
            NumberOfKiloMeters = c.NumberOfKiloMeters,
            TotalWeight = c.TotalWeight,
            Status = c.CurrentState,
            CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
        }).ToList();

        var model = new ManagementPageViewModel<SubscriptionPackageRowItem>
        {
            PageTitle = "Subscription Package Management",
            PageDescription = "Manage subscription packages for customers.",
            AddButtonText = "Add New Package",
            AddButtonController = "SubscriptionPackage",
            RecordLabel = "records",
            RecordIcon = "fa-cubes",
            SortedBy = "Package Name",
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
    public IActionResult Create() => View(new SubscriptionPackageFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionPackageFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CreateSubscriptionPackageDto
        {
            PackageName = model.PackageName,
            ShipimentCount = model.ShipimentCount,
            NumberOfKiloMeters = model.NumberOfKiloMeters,
            TotalWeight = model.TotalWeight
        };

        var result = await _subscriptionPackageService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create subscription package.");
            return View(model);
        }

        var createdId = result.Value.Id;
        await _subscriptionPackageService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "Subscription package created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _subscriptionPackageService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "Subscription package not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new SubscriptionPackageFormViewModel
        {
            Id = dto.Id,
            PackageName = dto.PackageName ?? string.Empty,
            ShipimentCount = dto.ShipimentCount,
            NumberOfKiloMeters = dto.NumberOfKiloMeters,
            TotalWeight = dto.TotalWeight,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SubscriptionPackageFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new UpdateSubscriptionPackageDto
        {
            PackageName = model.PackageName,
            ShipimentCount = model.ShipimentCount,
            NumberOfKiloMeters = model.NumberOfKiloMeters,
            TotalWeight = model.TotalWeight
        };

        var updateResult = await _subscriptionPackageService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update subscription package.");
            return View(model);
        }

        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _subscriptionPackageService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "Subscription package updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _subscriptionPackageService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "Subscription package deleted successfully." : "Failed to delete subscription package.";
        return RedirectToAction("Index");
    }
}
