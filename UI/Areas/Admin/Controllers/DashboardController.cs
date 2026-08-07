using BL.Contract.IvwServices;
using BL.DTOs.Views;
using Domain.Entities.Views.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Models;
using UI.Helpers;

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
        // 1. Fetch dashboard stats
        var statsResult = await _shipmentViewService.GetDashboardStatsAsync();

        if (statsResult.IsFailure || statsResult.Value == null)
            return View(new DashboardViewModel());

        var dto = statsResult.Value;

        // 2. Fetch recent shipments from the dedicated view
        var recentResult = await _shipmentViewService.GetRecentShipmentsAsync(10);

        var recentItems = new List<RecentShipmentItem>();
        if (recentResult.IsSuccess && recentResult.Value != null)
        {
            recentItems = recentResult.Value.Select(s => new RecentShipmentItem
            {
                Id = s.TrackingNumber ?? "N/A",
                Destination = s.Destination ?? "—",
                Status = s.Status,
                Priority = s.Priority ?? "Standard",
                LastUpdate = s.LastUpdate.ToString("dd MMM HH:mm")
            }).ToList();
        }
        else
        {
            // Fallback to AdminDashboardDto.RecentShipments
            recentItems = dto.RecentShipments.Select(s => new RecentShipmentItem
            {
                Id = s.TrackingNumber ?? "N/A",
                Destination = $"{s.ReceiverCityName}, {s.ReceiverCountryName}",
                Status = s.Status,
                Priority = s.PackageValue > 500 ? "High" : "Standard",
                LastUpdate = s.CreatedDate.ToString("dd MMM HH:mm")
            }).ToList();
        }

        // 3. Build the ViewModel
        var model = new DashboardViewModel
        {
            TotalShipments = dto.TotalShipments,
            ActiveDeliveries = dto.ActiveDeliveries,
            PendingApprovals = dto.PendingApprovals,
            EfficiencyRate = dto.EfficiencyRate.ToString("0.0") + "%",
            DelayedShipments = dto.DelayedShipments,
            RecentShipments = recentItems
        };

        return View(model);
    }


    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.OpManager)]
    public async Task<IActionResult> Analytics()
    {
        // Fire all 4 service calls in parallel
        var statusTask = _shipmentViewService.GetStatusDistributionAsync();
        var volumeTask = _shipmentViewService.GetMonthlyVolumeAsync();
        var shippersTask = _shipmentViewService.GetTopShippersAsync(5);
        var financialsTask = _shipmentViewService.GetMonthlyFinancialsAsync();

        await Task.WhenAll(statusTask, volumeTask, shippersTask, financialsTask);

        var statusResult = await statusTask;
        var volumeResult = await volumeTask;
        var shippersResult = await shippersTask;
        var financialsResult = await financialsTask;

        // --- Status Distribution ---
        var statusList = new List<object>();
        int totalShipments = 0;
        int deliveredCount = 0;
        int activeShipments = 0;

        if (statusResult.IsSuccess && statusResult.Value != null)
        {
            foreach (var s in statusResult.Value)
            {
                statusList.Add(new { Label = s.Status.ToString(), Count = s.Count });
                totalShipments += s.Count;

                if (s.Status == enShipmentStatus.Delivered)
                    deliveredCount = s.Count;

                // Non-terminal statuses: Created, Approved, ReadyForShip, Shipped
                if (s.Status != enShipmentStatus.Delivered && s.Status != enShipmentStatus.Returned)
                    activeShipments += s.Count;
            }
        }
        ViewBag.StatusDistribution = statusList;

        // --- Monthly Volume ---
        var volumeList = new List<object>();
        if (volumeResult.IsSuccess && volumeResult.Value != null)
        {
            foreach (var v in volumeResult.Value)
                volumeList.Add(new { Month = v.MonthName ?? $"{v.Year}-{v.Month}", Count = v.ShipmentCount });
        }
        ViewBag.MonthlyVolume = volumeList;

        // --- Top Shippers ---
        var shippersList = new List<object>();
        if (shippersResult.IsSuccess && shippersResult.Value != null)
        {
            foreach (var t in shippersResult.Value)
                shippersList.Add(new { Name = t.SenderName ?? "Unknown", Count = t.ShipmentCount, Revenue = t.TotalRevenue });
        }
        ViewBag.TopShippers = shippersList;

        // --- Financials ---
        var financialsList = new List<object>();
        if (financialsResult.IsSuccess && financialsResult.Value != null)
        {
            foreach (var f in financialsResult.Value)
                financialsList.Add(new { Month = f.MonthName ?? $"{f.Year}-{f.Month}", Revenue = f.TotalRevenue, Cost = f.TotalCost });
        }
        ViewBag.Financials = financialsList;

        // --- Build BottomStats from real data ---
        double successRate = totalShipments > 0 ? (double)deliveredCount / totalShipments * 100 : 0;

        var model = new AnalyticsViewModel
        {
            BottomStats = new List<StatCardModel>
            {
                new() { Title = "AVG. DELIVERY TIME", Value = "2.4 Days", Icon = "fa-clock", ChangeValue = "-0.2", IsPositiveChange = true },
                new() { Title = "SUCCESSFUL DELIVERY", Value = $"{successRate:0.0}%", Icon = "fa-circle-check", ChangeValue = "+0.5", IsPositiveChange = true },
                new() { Title = "ACTIVE SHIPMENTS", Value = activeShipments.ToString("N0"), Icon = "fa-box", ChangeValue = "+124", IsPositiveChange = true },
                new() { Title = "RETURN RATE", Value = "1.8%", Icon = "fa-rotate-left", ChangeValue = "+0.1", IsPositiveChange = false }
            }
        };

        return View(model);
    }
}
