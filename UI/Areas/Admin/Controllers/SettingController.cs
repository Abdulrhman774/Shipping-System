using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class SettingController : BaseAdminController
{
    [HttpGet]
    public IActionResult Edit()
    {
        return View(new SystemSettingsViewModel { KilometerRate = 5.50, KilogramRate = 2.25 });
    }

    [HttpPost]
    public IActionResult Edit(SystemSettingsViewModel model)
    {
        return RedirectToAction("Edit");
    }
}
