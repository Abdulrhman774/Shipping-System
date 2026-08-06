namespace UI.Areas.Admin.Models;

public class CarrierRowItem
{
    public Guid Id { get; set; }
    public string CarrierName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;
}
