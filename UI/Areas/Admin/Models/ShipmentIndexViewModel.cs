namespace UI.Areas.Admin.Models;

public class ShipmentIndexViewModel
{
    public List<ShipmentRowItem> Shipments { get; set; } = new();
    public PaginationModel Pagination { get; set; } = new();
}

public class ShipmentRowItem
{
    public int RowNumber { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ShippingDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
