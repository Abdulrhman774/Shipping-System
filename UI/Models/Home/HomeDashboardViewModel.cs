using BL.DTOs.Shipment;

namespace UI.Models.Home;

public class HomeDashboardViewModel
{
    // ── Statistics ────────────────────────────────────────────────
    public int TotalShipments { get; set; }

    /// <summary>Active (enEntityState = 1)</summary>
    public int PendingShipments { get; set; }

    /// <summary>Inactive (enEntityState = 2)</summary>
    public int InTransitShipments { get; set; }

    /// <summary>Deleted (enEntityState = 3)</summary>
    public int DeliveredShipments { get; set; }

    // ── Recent Shipments Table ────────────────────────────────────
    public List<DashboardShipmentRow> RecentShipments { get; set; } = new();
}

public class DashboardShipmentRow
{
    public Guid         Id             { get; set; }
    public string       TrackingNumber { get; set; } = "N/A";
    public Guid         SenderId       { get; set; }
    public Guid         ReceiverId     { get; set; }
    public DateTime     ShippingDate   { get; set; }
    public string       Status         { get; set; } = string.Empty;
    public string       BadgeClass     { get; set; } = string.Empty;
}
