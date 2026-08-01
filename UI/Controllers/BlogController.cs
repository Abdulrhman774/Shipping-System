// UI/Controllers/BlogController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            // TODO: جلب قائمة المقالات من الـ API
            return View();
        }

        public IActionResult Details(Guid id)
        {
            // TODO: جلب تفاصيل المقال حسب الـ id
            return View();
        }
    }
}