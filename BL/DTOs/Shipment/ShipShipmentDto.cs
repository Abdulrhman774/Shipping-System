namespace BL.DTOs.Shipment;

/// <summary>
/// Required when transitioning a shipment to the Shipped status.
/// </summary>
public class ShipShipmentDto
{
    /// <summary>The carrier assigned to deliver this shipment.</summary>
    public Guid CarrierId { get; set; }

    /// <summary>Optional note recorded in the status history.</summary>
    public string? Note { get; set; }
}