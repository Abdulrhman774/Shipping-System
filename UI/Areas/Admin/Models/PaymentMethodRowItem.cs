namespace UI.Areas.Admin.Models;

public class PaymentMethodRowItem
{
    public Guid Id { get; set; }
    public string MethodEname { get; set; } = string.Empty;
    public string MethdAname { get; set; } = string.Empty;
    public double? Commission { get; set; }
    public enEntityState Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
