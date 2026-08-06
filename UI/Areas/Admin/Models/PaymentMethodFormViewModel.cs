namespace UI.Areas.Admin.Models;

public class PaymentMethodFormViewModel
{
    public Guid? Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public double Commission { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
}
