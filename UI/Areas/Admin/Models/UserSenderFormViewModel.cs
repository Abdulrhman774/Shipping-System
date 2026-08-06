using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Areas.Admin.Models;

public class UserSenderFormViewModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? OtherAddress { get; set; }
    public bool IsDefaultAddress { get; set; }
    public Guid CityId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
    public List<SelectListItem> Cities { get; set; } = new();
}
