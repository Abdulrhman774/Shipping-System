using BL.Contract.IvwServices;
using BL.DTOs.User;
using BL.DTOs.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using UI.Models;
using UI.Models.Profile;
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

            var userDto = userResponse.Success ? userResponse.Data : null;
            var userData = userDto != null ? new UserProfileEditViewModel
            {
                FirstName = userDto.FirstName ?? string.Empty,
                SecondName = userDto.SecondName ?? string.Empty,
                ThirdName = userDto.ThirdName,
                LastName = userDto.LastName ?? string.Empty,
                DateOfBirth = userDto.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Gender = userDto.Gender,
                ImageUrl = userDto.ImageUrl,
                Email = userDto.Email ?? string.Empty,
                PhoneNumber = userDto.PhoneNumber,
                ImageFile = null
            } : null;

            var model = new ProfileViewModel
            {
                UserData = userData!,
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


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> ChangeUserDate(UpdateUserDto model, IFormFile? fileToUpload)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        TempData["ErrorMessage"] = "Please correct the errors.";
    //        return RedirectToAction("Index");
    //    }

    //    try
    //    {
    //        var userId = GetCurrentUserId();
    //        if (string.IsNullOrEmpty(userId))
    //            return RedirectToAction("Login", "Account");

    //        if (fileToUpload != null && fileToUpload.Length > 0)
    //        {
    //            // إنشاء اسم ملف فريد
    //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileToUpload.FileName);
    //            // المسار الذي سيُحفظ فيه (تأكد من وجود مجلد uploads في wwwroot)
    //            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    //            if (!Directory.Exists(uploadsFolder))
    //                Directory.CreateDirectory(uploadsFolder);

    //            var filePath = Path.Combine(uploadsFolder, fileName);
    //            using (var stream = new FileStream(filePath, FileMode.Create))
    //            {
    //                await fileToUpload.CopyToAsync(stream);
    //            }

    //            model.ImageUrl = "/uploads/" + fileName;
    //        }
    //        else
    //        {
    //            // إذا لم يتم رفع صورة جديدة، نحتفظ بالصورة القديمة (تم إرسالها من الـ View)
    //            // لكن إذا لم يكن هناك صورة، نتركها كما هي.
    //        }

    //        var response = await _apiClient.PutAsync<object>($"Api/User/{userId}", model);

    //        if (response.Success)
    //        {
    //            TempData["SuccessMessage"] = "Profile updated successfully!";
    //        }
    //        else
    //        {
    //            TempData["ErrorMessage"] = response.Error ?? "Failed to update profile.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error updating user data");
    //        TempData["ErrorMessage"] = "An error occurred while updating your profile.";
    //    }

    //    return RedirectToAction("Index");
    //}


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserData(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the errors.";
            return RedirectToAction("Index");
        }

        var userData = model.UserData;
        if (userData == null)
        {
            TempData["ErrorMessage"] = "User data is missing.";
            return RedirectToAction("Index");
        }

        try
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            // 1. معالجة رفع الصورة (إذا تم اختيار ملف)
            if (userData.ImageFile != null && userData.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userData.ImageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userData.ImageFile.CopyToAsync(stream);
                }
                userData.ImageUrl = "/uploads/" + fileName;
            }

            // 2. تحويل ViewModel إلى DTO للإرسال إلى الـ API
            var updateDto = new UpdateUserDto
            {
                FirstName = userData.FirstName,
                SecondName = userData.SecondName,
                ThirdName = userData.ThirdName,
                LastName = userData.LastName,
                DateOfBirth = DateOnly.FromDateTime(userData.DateOfBirth),
                Gender = userData.Gender,
                ImageUrl = userData.ImageUrl, // إذا لم يتم رفع صورة جديدة، ستبقى القيمة القديمة
                Email = userData.Email,
                PhoneNumber = userData.PhoneNumber
            };

            // 3. إرسال الطلب إلى الـ API
            var response = await _apiClient.PutAsync<object>($"Api/User/{userId}", updateDto);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = response.Error ?? "Failed to update profile.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user data");
            TempData["ErrorMessage"] = "An error occurred while updating your profile.";
        }

        return RedirectToAction("Index");
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileViewModel model)
    {
        // التحقق اليدوي من صحة كلمة المرور
        var isValid = true;

        if (string.IsNullOrWhiteSpace(model.CurrentPassword))
        {
            ModelState.AddModelError("CurrentPassword", "Current password is required.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(model.NewPassword))
        {
            ModelState.AddModelError("NewPassword", "New password is required.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
        {
            ModelState.AddModelError("ConfirmPassword", "Please confirm your new password.");
            isValid = false;
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
            isValid = false;
        }

        if (model.NewPassword.Length < 9)
        {
            ModelState.AddModelError("NewPassword", "Password must be at least 9 characters.");
            isValid = false;
        }

        if (!isValid)
        {
            TempData["ErrorMessage"] = "Please correct the password errors.";
            return RedirectToAction("Index");
        }

        // ... باقي الكود (إرسال الطلب إلى الـ API)
        try
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var response = await _apiClient.PostAsync<object>("Api/Auth/ChangePassword", new
            {
                UserId = userId,
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword
            });

            if (response.Success)
                TempData["SuccessMessage"] = "Password changed successfully!";
            else
                TempData["ErrorMessage"] = response.Error ?? "Failed to change password.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            TempData["ErrorMessage"] = "An error occurred while changing your password.";
        }

        return RedirectToAction("Index");
    }

}