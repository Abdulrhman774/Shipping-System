using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Areas.Admin.Models;

public class UserSubscriptionFormViewModel
{
    public Guid? Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid PackageId { get; set; }
    public DateTime SubscriptionDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
    public List<SelectListItem> Users { get; set; } = new();
    public List<SelectListItem> Packages { get; set; } = new();
}
