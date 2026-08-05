// UI/Controllers/TrackingController.cs
using BL.Contract.IvwServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UI.Models;

namespace UI.Controllers;

[Authorize]
public class TrackingController : Controller
{
    private readonly IShipmentViewService _shipmentViewService;
    private readonly ILogger<TrackingController> _logger;

    public TrackingController(
        IShipmentViewService shipmentViewService,
        ILogger<TrackingController> logger)
    {
        _shipmentViewService = shipmentViewService;
        _logger = logger;
    }

    // GET: /Tracking
    public IActionResult Index()
    {
        return View(new TrackingViewModel());
    }

    // POST: /Tracking/Result
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Result(string trackingNumber)
    {
        if (string.IsNullOrEmpty(trackingNumber))
        {
            TempData["ErrorMessage"] = "Please enter a tracking number.";
            return RedirectToAction("Index");
        }

        try
        {
            if (!Guid.TryParse(GetCurrentUserId(), out var userGuid))
            {
                TempData["ErrorMessage"] = "Invalid user ID.";
                return RedirectToAction("Index");
            }

            var userId = userGuid;

            var result = await _shipmentViewService.GetShipmentByTrackingNumberAsync(trackingNumber, userId);

            if (result.IsSuccess && result.Value != null)
            {
                var model = new TrackingViewModel
                {
                    TrackingNumber = trackingNumber,
                    Shipment = result.Value,
                    IsFound = true
                };
                return View(model);
            }

            var notFoundModel = new TrackingViewModel
            {
                TrackingNumber = trackingNumber,
                IsFound = false,
                ErrorMessage = "Shipment not found. Please check the tracking number."
            };
            return View(notFoundModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking shipment {TrackingNumber}", trackingNumber);
            var errorModel = new TrackingViewModel
            {
                TrackingNumber = trackingNumber,
                IsFound = false,
                ErrorMessage = "An error occurred while tracking your shipment. Please try again."
            };
            return View(errorModel);
        }
    }

    // GET: /Tracking/Result
    [HttpGet]
    public IActionResult Result()
    {
        return RedirectToAction("Index");
    }

    private string GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("UserId");

        return userIdString ?? string.Empty;
    }
}