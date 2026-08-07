using BL.Contract.IvwServices;
using BL.DTOs.Views;
using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    private readonly IShipmentViewService _shipmentViewService;

    public DashboardController(IShipmentViewService shipmentViewService)
    {
        _shipmentViewService = shipmentViewService;
    }

    public async Task<IActionResult> Index()
    {
        // 1. استدعاء الخدمة لجلب البيانات الحقيقية
        var result = await _shipmentViewService.GetDashboardStatsAsync();

        if (result.IsFailure || result.Value == null)
        {
            // في حالة الخطأ، أرجع ViewModel فارغ أو رسالة خطأ
            return View(new DashboardViewModel());
        }

        var dto = result.Value;

        // 2. تحويل AdminDashboardDto إلى DashboardViewModel
        var model = new DashboardViewModel
        {
            TotalShipments = dto.TotalShipments,
            ActiveDeliveries = dto.ActiveDeliveries,
            PendingApprovals = dto.PendingApprovals,
            EfficiencyRate = dto.EfficiencyRate.ToString("0.0") + "%", // تحويل الرقم إلى نص مع %
            DelayedShipments = dto.DelayedShipments,
            RecentShipments = dto.RecentShipments.Select(s => new RecentShipmentItem
            {
                Id = s.TrackingNumber ?? "N/A",                 // استخدام TrackingNumber
                Destination = $"{s.ReceiverCityName}, {s.ReceiverCountryName}", // دمج المدينة والدولة
                Status = s.Status,                 // ✅ عيّن الـ Enum مباشرة
                Priority = s.PackageValue > 500 ? "High" : "Standard", // تحديد الأولوية حسب قيمة الشحنة
                LastUpdate = s.CreatedDate.ToString("dd MMM HH:mm") // تنسيق التاريخ
            }).ToList()
        };

        return View(model);
    }
    public IActionResult Analytics()
    {
        var model = new AnalyticsViewModel
        {
            BottomStats = new List<StatCardModel>
            {
                new() { Title = "AVG. DELIVERY TIME", Value = "2.4 Days", Icon = "fa-clock", ChangeValue = "-0.2", IsPositiveChange = false },
                new() { Title = "SUCCESSFUL DELIVERY", Value = "98.2%", Icon = "fa-circle-check", ChangeValue = "+0.5", IsPositiveChange = true },
                new() { Title = "ACTIVE SHIPMENTS", Value = "1,245", Icon = "fa-box", ChangeValue = "+124", IsPositiveChange = true },
                new() { Title = "RETURN RATE", Value = "1.8%", Icon = "fa-rotate-left", ChangeValue = "+0.1", IsPositiveChange = true }
            }
        };
        return View(model);
    }
}
