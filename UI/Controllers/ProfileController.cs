using BL.Contract.IvwServices;
using BL.DTOs.User;
using BL.DTOs.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using UI.Models;
using UI.Services;

namespace UI.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly GenericApiClient _apiClient;
    private readonly IShipmentViewService _shipmentViewService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        GenericApiClient apiClient,
        IShipmentViewService shipmentViewService,
        ILogger<ProfileController> logger)
    {
        _apiClient = apiClient;
        _shipmentViewService = shipmentViewService;
        _logger = logger;
    }

    private string GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("UserId");

        return userIdString ?? string.Empty;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. جلب بيانات المستخدم
            var userResponse = await _apiClient.GetAsync<UserDto>($"Api/User/{userId}");

            // 2. جلب كل الشحنات
            var shipmentsResult = await _shipmentViewService.GetShipmentsByUserAsync(Guid.Parse(userId));
            var allShipments = shipmentsResult.IsSuccess
                ? shipmentsResult.Value.ToList()
                : new List<ShipmentDetailsDto>();

            // 3. فصل الشحنات النشطة (Active) والتاريخ (كل الشحنات)
            var activeShipments = allShipments
                .Where(x => x.CurrentState == enEntityState.Active)
                .ToList();

            var model = new ProfileViewModel
            {
                User = userResponse.Success ? userResponse.Data : null,
                ActiveShipments = activeShipments,
                ShipmentHistory = allShipments
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile");
            TempData["ErrorMessage"] = "Failed to load profile.";
            return View(new ProfileViewModel());
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the errors.";
            return RedirectToAction("Index");
        }

        try
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _apiClient.PutAsync<object>($"Api/User/{userId}", model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = response.Error ?? "Failed to update profile.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            TempData["ErrorMessage"] = "An error occurred while updating your profile.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileViewModel model)
    {
        // ✅ تحقق من صحة الـ Model (الـ Password)
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the password errors.";
            return RedirectToAction("Index");
        }

        try
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _apiClient.PostAsync<object>("Api/Auth/ChangePassword", new
            {
                UserId = userId,
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword
            });

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Password changed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = response.Error ?? "Failed to change password.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            TempData["ErrorMessage"] = "An error occurred while changing your password.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TrackShipment(string trackingNumber)
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
                // ✅ Logging للتأكد من البيانات
                _logger.LogInformation($"Tracking found: {trackingNumber}, ShippingType: {result.Value.ShippingTypeName}, DeliveryDate: {result.Value.DeliveryDate}");

                TempData["TrackingResult"] = JsonConvert.SerializeObject(result.Value);
                TempData["TrackingFound"] = "true";
            }
            else
            {
                TempData["ErrorMessage"] = "Shipment not found. Please check the tracking number.";
                TempData["TrackingFound"] = "false";
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking shipment {TrackingNumber}", trackingNumber);
            TempData["ErrorMessage"] = "An error occurred while tracking your shipment.";
            return RedirectToAction("Index");
        }
    }
}