using BL.DTOs.Views;

namespace UI.Models;

public class TrackingViewModel
{
    public string? TrackingNumber { get; set; }
    public ShipmentDetailsDto? Shipment { get; set; }
    public bool IsFound { get; set; }
    public string? ErrorMessage { get; set; }
}
