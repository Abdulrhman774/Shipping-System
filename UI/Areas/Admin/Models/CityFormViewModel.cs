using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Areas.Admin.Models;

public class CityFormViewModel
{
    public Guid? Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
    public List<SelectListItem> Countries { get; set; } = new();
}
