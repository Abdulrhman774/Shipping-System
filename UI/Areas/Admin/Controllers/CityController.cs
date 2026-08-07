using BL.Contract.IServices;
using BL.DTOs.City;
using Domain.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

public class CityController : BaseAdminController
{
    private readonly ICityService _cityService;
    private readonly ICountryService _countryService;

    public CityController(ICityService cityService, ICountryService countryService)
    {
        _cityService = cityService;
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

        Expression<Func<TbCity, bool>>? filter = null;

        if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => (c.CityEname.Contains(search) || c.CityAname.Contains(search)) && c.CurrentState == statusEnum;
        }
        else if (!string.IsNullOrEmpty(search))
        {
            filter = c => c.CityEname.Contains(search) || c.CityAname.Contains(search);
        }
        else if (!string.IsNullOrEmpty(appliedStatus))
        {
            var statusEnum = appliedStatus == "Active" ? enEntityState.Active : enEntityState.Inactive;
            filter = c => c.CurrentState == statusEnum;
        }

        var result = await _cityService.GetPagedAsync(page, pageSize, filter);

        if (result.IsFailure || result.Value == null)
            return View(new ManagementPageViewModel<CityRowItem>());

        var pagedResult = result.Value;

        var countriesResult = await _countryService.GetAllAsync();
        var countries = countriesResult.IsSuccess && countriesResult.Value != null ? countriesResult.Value.ToList() : new();

        var items = pagedResult.Items.Select(c => {
            var country = countries.FirstOrDefault(x => x.Id == c.CountryId);
            var countryName = country != null ? (!string.IsNullOrEmpty(country.CountryEname) ? country.CountryEname : country.CountryAname) : string.Empty;
            return new CityRowItem
            {
                Id = c.Id,
                CityAname = c.CityAname ?? string.Empty,
                CityEname = c.CityEname ?? string.Empty,
                CountryName = countryName,
                Status = c.CurrentState,
                CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
            };
        }).ToList();

        var model = new ManagementPageViewModel<CityRowItem>
        {
            PageTitle = "City Management",
            PageDescription = "Manage cities across countries.",
            AddButtonText = "Add New City",
            AddButtonController = "City",
            RecordLabel = "records",
            RecordIcon = "fa-city",
            SortedBy = "City Name",
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
    public async Task<IActionResult> Create()
    {
        var model = new CityFormViewModel();
        var countriesResult = await _countryService.GetAllAsync();
        if (countriesResult.IsSuccess && countriesResult.Value != null)
        {
            model.Countries = countriesResult.Value.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = !string.IsNullOrEmpty(c.CountryEname) ? c.CountryEname : c.CountryAname
            }).ToList();
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var countriesResult = await _countryService.GetAllAsync();
            if (countriesResult.IsSuccess && countriesResult.Value != null)
            {
                model.Countries = countriesResult.Value.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = !string.IsNullOrEmpty(c.CountryEname) ? c.CountryEname : c.CountryAname
                }).ToList();
            }
            return View(model);
        }

        var dto = new CreateCityDto
        {
            CityAname = model.CityAname,
            CityEname = model.CityEname,
            CountryId = model.CountryId
        };

        var result = await _cityService.AddAsync(dto, autoSave: true);

        if (result.IsFailure)
        {
            ModelState.AddModelError("", "Failed to create city.");
            return View(model);
        }

        var createdId = result.Value.Id;
        await _cityService.ChangeStatusAsync(createdId, model.IsActive ? enEntityState.Active : enEntityState.Inactive);

        TempData["SuccessMessage"] = "City created successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _cityService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = "City not found.";
            return RedirectToAction("Index");
        }

        var dto = result.Value;
        var model = new CityFormViewModel
        {
            Id = dto.Id,
            CityAname = dto.CityAname ?? string.Empty,
            CityEname = dto.CityEname ?? string.Empty,
            CountryId = dto.CountryId,
            IsActive = dto.CurrentState == enEntityState.Active
        };

        var countriesResult = await _countryService.GetAllAsync();
        if (countriesResult.IsSuccess && countriesResult.Value != null)
        {
            model.Countries = countriesResult.Value.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = !string.IsNullOrEmpty(c.CountryEname) ? c.CountryEname : c.CountryAname
            }).ToList();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CityFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var countriesResult = await _countryService.GetAllAsync();
            if (countriesResult.IsSuccess && countriesResult.Value != null)
            {
                model.Countries = countriesResult.Value.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = !string.IsNullOrEmpty(c.CountryEname) ? c.CountryEname : c.CountryAname
                }).ToList();
            }
            return View(model);
        }

        var dto = new UpdateCityDto
        {
            CityAname = model.CityAname,
            CityEname = model.CityEname,
            CountryId = model.CountryId
        };

        var updateResult = await _cityService.UpdateAsync(model.Id, dto, autoSave: true);
        if (updateResult.IsFailure)
        {
            ModelState.AddModelError("", "Failed to update city.");
            return View(model);
        }

        var newState = model.IsActive ? enEntityState.Active : enEntityState.Inactive;
        await _cityService.ChangeStatusAsync(model.Id, newState);

        TempData["SuccessMessage"] = "City updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _cityService.DeleteAsync(id, autoSave: true);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess ? "City deleted successfully." : "Failed to delete city.";
        return RedirectToAction("Index");
    }
}
