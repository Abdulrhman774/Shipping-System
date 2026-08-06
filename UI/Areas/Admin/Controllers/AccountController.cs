using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class AccountController : BaseAdminController
{
    [HttpGet]
    public IActionResult Profile()
    {
        var model = new AdminProfileViewModel
        {
            FirstName = "Alex",
            LastName = "Rivers",
            Email = "alex.rivers@shipmentflow.com",
            PhoneNumber = "+1 (555) 234-5678",
            Role = "Admin",
            AvatarInitials = "AR",
            JoinDate = "Jan 15, 2024",
            Department = "Operations Management",
            TimeZone = "UTC+3 (Eastern Europe)"
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Profile(AdminProfileViewModel model)
    {
        TempData["SuccessMessage"] = "Profile updated successfully!";
        return RedirectToAction("Profile");
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        TempData["SuccessMessage"] = "Password changed successfully!";
        return RedirectToAction("Profile");
    }
}
