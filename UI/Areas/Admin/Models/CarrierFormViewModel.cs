namespace UI.Areas.Admin.Models;

public class CarrierFormViewModel
{
    public Guid? Id { get; set; }
    public string CarrierName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
}
