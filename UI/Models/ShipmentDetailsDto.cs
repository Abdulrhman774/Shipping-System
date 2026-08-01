namespace UI.Models;

public class ShipmentDetailsModel
{
    public Guid ShipmentId { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public decimal PackageValue { get; set; }
    public decimal ShippingRate { get; set; }

    // Sender
    public string? SenderName { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderPhone { get; set; }
    public string? SenderAddress { get; set; }
    public string? SenderCityName { get; set; }
    public string? SenderCountryName { get; set; }

    // Receiver
    public string? ReceiverName { get; set; }
    public string? ReceiverEmail { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverAddress { get; set; }
    public string? ReceiverCityName { get; set; }
    public string? ReceiverCountryName { get; set; }

    // Status
    public enEntityState CurrentState { get; set; }
    public DateTime CreatedDate { get; set; }
}
