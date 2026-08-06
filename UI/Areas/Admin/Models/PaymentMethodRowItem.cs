namespace UI.Areas.Admin.Models;

public class PaymentMethodRowItem
{
    public Guid Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string Commission { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Icon { get; set; } = "fa-credit-card";
}
