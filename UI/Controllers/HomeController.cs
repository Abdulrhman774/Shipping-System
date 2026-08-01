using BL.Contract.IvwServices;
using BL.DTOs.Shipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using UI.Models;
using UI.Models.Home;
using UI.Services;

namespace UI.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MvcShipmentService _shipmentService;
        private readonly IShipmentViewService _shipmentViewService;

        public HomeController(ILogger<HomeController> logger, MvcShipmentService shipmentService, IShipmentViewService shipmentViewService)
        {
            _logger = logger;
            _shipmentService = shipmentService;
            _shipmentViewService = shipmentViewService; 
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            // 1. إحصائيات الشحنات
            var statsResult = await _shipmentViewService.GetShipmentStatsAsync();
            ViewBag.ShipmentStats = statsResult.IsSuccess ? statsResult.Value : null;

            // 2. آخر 5 شحنات
            var recentResult = await _shipmentViewService.GetShipmentsByUserAsync(Guid.Parse(userId));
            ViewBag.RecentShipments = recentResult.IsSuccess ? recentResult.Value.Take(5) : null;

            // 3. الشحنات الشهرية
            var monthlyResult = await _shipmentViewService.GetMonthlyShipmentsAsync();
            ViewBag.MonthlyShipments = monthlyResult.IsSuccess ? monthlyResult.Value.Take(12) : null;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static string GetStatusLabel(int state) => state switch
        {
            1 => "Pending",
            2 => "In Transit",
            3 => "Delivered",
            _ => "Unknown"
        };

        private static string GetBadgeClass(int state) => state switch
        {
            1 => "badge-pending",
            2 => "badge-transit",
            3 => "badge-delivered",
            _ => "badge-unknown"
        };

        // ✅ إضافة هذه الطريقة
        private string GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("UserId");

            return userIdString ?? string.Empty;
        }

        // صفحة من نحن (About)
        public IActionResult About()
        {
            return View();
        }

        // صفحة الخدمات (Services)
        public IActionResult Services()
        {
            return View();
        }

        // تفاصيل خدمة معينة (سيتم نقلها لاحقاً إلى ServicesController)
        public IActionResult ServiceDetails(int id)
        {
            // يمكنك هنا تمرير البيانات الخاصة بالخدمة id
            return View("ServiceDetails");
        }

        // صفحة المدونة (Blog)
        public IActionResult Blog()
        {
            return View("Blog");
        }

        // تفاصيل مقال معين
        public IActionResult BlogDetails(int id)
        {
            return View("BlogDetails");
        }

        // صفحة الاتصال (Contact)
        public IActionResult Contact()
        {
            return View();
        }

        // صفحة التتبع (Tracking)
        public IActionResult Tracking()
        {
            return View();
        }

        // نتيجة التتبع
        public IActionResult TrackingResult()
        {
            return View("TrackingResult");
        }

        // صفحة الملف الشخصي (Profile)
        public IActionResult Profile()
        {
            return View();
        }

        // صفحة الخطأ 404
        public IActionResult Error404()
        {
            return View();
        }

        // إضافة أكشن الـ Newsletter Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Please enter a valid email address.";
                return RedirectToAction("Index");
            }

            try
            {
                // TODO: استدعاء الـ API لحفظ الـ email
                // var response = await _apiClient.PostAsync<object>("Api/Newsletter/Subscribe", new { Email = email });
                // if (response.Success)
                // {
                //     TempData["SuccessMessage"] = "Thank you for subscribing!";
                // }
                // else
                // {
                //     TempData["ErrorMessage"] = "Failed to subscribe. Please try again.";
                // }

                // مؤقتاً
                TempData["SuccessMessage"] = "Thank you for subscribing!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing email {Email}", email);
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
            }

            return RedirectToAction("Index");
        }
    }
}
