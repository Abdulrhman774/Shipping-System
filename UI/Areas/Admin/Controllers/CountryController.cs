using BL.Contract.IServices;
using BL.DTOs.Country;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

public class CountryController : BaseAdminController
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
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

        Expression<Func<TbCountry, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => (c.CountryEname.Contains(search) || c.CountryAname.Contains(search)) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => c.CountryEname.Contains(search) || c.CountryAname.Contains(search);
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

        var result = await _countryService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<CountryRowItem>());

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(c => new CountryRowItem
        {
            Id = c.Id,
            CountryEname = c.CountryEname ?? string.Empty,
            CountryAname = c.CountryAname ?? string.Empty,
            Status = c.CurrentState,
            CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
        }).ToList();

        var model = new ManagementPageViewModel<CountryRowItem>
        {
            PageTitle = "Country Management",
            PageDescription = "Manage global destinations for regional logistics routing.",
            AddButtonText = "Add New Country",
            AddButtonController = "Country",
            RecordLabel = "records",
            RecordIcon = "fa-globe",
            SortedBy = "Country Name",
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
    public IActionResult Create() => View(new CountryFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CreateCountryDto
        {
            CountryEname = model.CountryEname,
            CountryAname = model.CountryAname
        };

        var result = await _countryService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create country.");
            return View(model);
        }

        var createdId = result.Value.Id;
        await _countryService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "Country created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _countryService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "Country not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new CountryFormViewModel
        {
            Id = dto.Id,
            CountryEname = dto.CountryEname ?? string.Empty,
            CountryAname = dto.CountryAname ?? string.Empty,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CountryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new UpdateCountryDto
        {
            CountryEname = model.CountryEname,
            CountryAname = model.CountryAname
        };

        var updateResult = await _countryService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update country.");
            return View(model);
        }

        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _countryService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "Country updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _countryService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "Country deleted successfully." : "Failed to delete country.";
        return RedirectToAction("Index");
    }
}
