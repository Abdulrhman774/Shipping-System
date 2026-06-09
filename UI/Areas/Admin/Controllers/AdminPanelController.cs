using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminPanelController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
