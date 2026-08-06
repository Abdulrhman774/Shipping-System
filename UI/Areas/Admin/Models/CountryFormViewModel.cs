namespace UI.Areas.Admin.Models;

public class CountryFormViewModel
{
    public Guid? Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
}
