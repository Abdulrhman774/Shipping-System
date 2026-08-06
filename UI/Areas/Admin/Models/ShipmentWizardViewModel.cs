namespace UI.Areas.Admin.Models;

public class ShipmentWizardViewModel
{
    public int CurrentStep { get; set; } = 1;
    // Step 1: Sender
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string PointOfContact { get; set; } = string.Empty;
    public string OtherAddressDetails { get; set; } = string.Empty;
    public bool IsDefaultSender { get; set; }
}
