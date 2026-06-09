using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UI.Areas.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                context.Result = RedirectToAction("AccessDenied", "Account", new { area = "" });
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
