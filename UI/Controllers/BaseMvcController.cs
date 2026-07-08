using BL.Common;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers;

/// <summary>
/// Generic MVC controller for entities exposed via BaseController<> on the WebApi side.
/// Handles Index, Details, Create, Edit, Delete using standard CRUD endpoints.
/// </summary>
public abstract class BaseMvcController<TDto, TCreateDto, TUpdateDto> : Controller
{
    protected readonly GenericApiClient _apiClient;
    protected readonly ILogger _logger;
    protected readonly string _apiEndpoint; // e.g. "Api/Carrier"

    protected BaseMvcController(GenericApiClient apiClient, ILogger logger, string apiEndpoint)
    {
        _apiClient = apiClient;
        _logger = logger;
        _apiEndpoint = apiEndpoint;
    }

    // GET: /Entity
    public virtual async Task<IActionResult> Index()
    {
        var response = await _apiClient.GetAsync<IEnumerable<TDto>>(_apiEndpoint);

        if (!response.Success)
        {
            TempData["ErrorMessage"] = response.Error;
            return View(Enumerable.Empty<TDto>());
        }

        return View(response.Data);
    }

    // GET: /Entity/Details/{id}
    public virtual async Task<IActionResult> Details(Guid id)
    {
        var response = await _apiClient.GetAsync<TDto>($"{_apiEndpoint}/{id}");

        if (!response.Success || response.Data == null)
        {
            TempData["ErrorMessage"] = response.Error ?? "Not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(response.Data);
    }

    // GET: /Entity/Create
    public virtual IActionResult Create() => View();

    // POST: /Entity/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var response = await _apiClient.PostAsync<object>(_apiEndpoint, dto);

        if (!response.Success)
        {
            ModelState.AddModelError("", response.Error ?? "Failed to create.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Entity/Edit/{id}
    public virtual async Task<IActionResult> Edit(Guid id)
    {
        var response = await _apiClient.GetAsync<TDto>($"{_apiEndpoint}/{id}");

        if (!response.Success || response.Data == null)
        {
            TempData["ErrorMessage"] = response.Error ?? "Not found.";
            return RedirectToAction(nameof(Index));
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(response.Data);
        var updateDto = Newtonsoft.Json.JsonConvert.DeserializeObject<TUpdateDto>(json);
        ViewBag.Id = id;

        return View(updateDto);
    }

    // POST: /Entity/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(Guid id, TUpdateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var response = await _apiClient.PutAsync<object>($"{_apiEndpoint}/{id}", dto!);

        if (!response.Success)
        {
            ModelState.AddModelError("", response.Error ?? "Failed to update.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Entity/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _apiClient.DeleteAsync($"{_apiEndpoint}/{id}");

        TempData[deleted ? "SuccessMessage" : "ErrorMessage"] =
            deleted ? "Deleted successfully." : "Failed to delete.";

        return RedirectToAction(nameof(Index));
    }
}