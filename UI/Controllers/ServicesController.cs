// UI/Controllers/ServicesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    [AllowAnonymous]
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(Guid id)
        {
            // TODO: جلب تفاصيل الخدمة حسب الـ id
            return View();
        }
    }
}