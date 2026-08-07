using Domain.Shared;

namespace UI.Areas.Admin.Models;

public class CityRowItem
{
    public Guid Id { get; set; }
    public string CityAname { get; set; } = string.Empty;
    public string CityEname { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public enEntityState Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
