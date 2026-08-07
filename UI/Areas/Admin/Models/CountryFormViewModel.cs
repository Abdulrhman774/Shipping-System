namespace UI.Areas.Admin.Models;

public class CountryFormViewModel
{
    public Guid Id { get; set; }
    public string CountryEname { get; set; } = string.Empty;
    public string CountryAname { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id != Guid.Empty;
}
