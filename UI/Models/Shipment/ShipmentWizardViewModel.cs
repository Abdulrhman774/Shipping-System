using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Models.Shipment;

public class ShipmentWizardViewModel
{
    public CreateUserSenderViewModel Sender { get; set; } = new();

    public CreateUserReceiverViewModel Receiver { get; set; } = new();

    public CreateShipmentViewModel Shipment { get; set; } = new();

    public List<SelectListItem>? Cities { get; set; }
}