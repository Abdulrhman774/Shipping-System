using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers

{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            // هذا السطر هو الذي يحدد مكان الـ View
            // سيبحث في: Views/Profile/Index.cshtml
            return View(); 
        }
    }
}