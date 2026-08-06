using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;

namespace UI.Areas.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            TotalShipments = 1284,
            ActiveDeliveries = 428,
            PendingApprovals = 24,
            EfficiencyRate = "94.2%",
            DelayedShipments = 12,
            RecentShipments = new List<RecentShipmentItem>
            {
                new() { Id = "SHP-9821", Destination = "Singapore (SIN)", Status = "In Transit", Priority = "High", LastUpdate = "2h ago" },
                new() { Id = "SHP-8472", Destination = "London (LHR)", Status = "Pending", Priority = "Medium", LastUpdate = "5h ago" },
                new() { Id = "SHP-7731", Destination = "Tokyo (NRT)", Status = "Delivered", Priority = "Low", LastUpdate = "1d ago" },
                new() { Id = "SHP-6210", Destination = "New York (JFK)", Status = "Action Required", Priority = "High", LastUpdate = "10m ago" },
                new() { Id = "SHP-5592", Destination = "Berlin (BER)", Status = "In Transit", Priority = "Medium", LastUpdate = "4h ago" }
            }
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
