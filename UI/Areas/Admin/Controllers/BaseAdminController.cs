using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers;

/// <summary>
/// Base controller for all Admin area controllers.
/// Any of the 4 admin roles can enter the area.
/// Individual actions restrict further where needed.
/// </summary>
[Area("Admin")]
[Authorize(Policy = "AdminPolicy")] // سطر واحد فقط!
public abstract class BaseAdminController : Controller
{
}
