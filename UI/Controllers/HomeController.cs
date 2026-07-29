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

        public HomeController(ILogger<HomeController> logger, MvcShipmentService shipmentService)
        {
            _logger = logger;
            _shipmentService = shipmentService;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Fetch all shipments from the API
            var response = await _shipmentService.GetAllShipmentsAsync();

            if (!response.Success || response.Data is null)
            {
                _logger.LogWarning("Failed to load shipments for dashboard. Error: {Error}", response.Error);
                return View(new HomeDashboardViewModel());
            }

            var allShipments = response.Data;

            // 2. Build statistics (enEntityState: Active=1, Inactive=2, Deleted=3)
            var model = new HomeDashboardViewModel
            {
                TotalShipments    = allShipments.Count,
                PendingShipments  = allShipments.Count(s => (int)s.CurrentState == 1),
                InTransitShipments= allShipments.Count(s => (int)s.CurrentState == 2),
                DeliveredShipments= allShipments.Count(s => (int)s.CurrentState == 3),

                // 3. Last 5 shipments ordered by CreatedDate descending
                RecentShipments = allShipments
                    .OrderByDescending(s => s.CreatedDate)
                    .Take(5)
                    .Select(s => new DashboardShipmentRow
                    {
                        Id             = s.Id,
                        TrackingNumber = s.TrackingNumber ?? "N/A",
                        SenderId       = s.SenderId,
                        ReceiverId     = s.ReceiverId,
                        ShippingDate   = s.ShippingDate,
                        Status         = GetStatusLabel((int)s.CurrentState),
                        BadgeClass     = GetBadgeClass((int)s.CurrentState)
                    })
                    .ToList()
            };

            return View(model);
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
    }
}
