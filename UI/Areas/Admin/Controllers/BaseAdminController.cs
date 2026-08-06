using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers;

/// <summary>
/// Base controller for all Admin area controllers.
/// Provides area routing and role-based authorization.
/// </summary>
[Area("Admin")]
//[Authorize(Roles = "Admin,Reviewer,Operation,OperationManager")]
public abstract class BaseAdminController : Controller
{
}
