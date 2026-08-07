namespace UI.Areas.Admin.Models;

public class PaymentMethodFormViewModel
{
    public Guid Id { get; set; }
    public string? MethodEname { get; set; }
    public string? MethdAname { get; set; }
    public double? Commission { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id != Guid.Empty;
}
