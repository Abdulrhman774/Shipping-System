namespace UI.Areas.Admin.Models;

public class CountryRowItem
{
    public Guid Id { get; set; }
    public string CountryEname { get; set; } = string.Empty;
    public string CountryAname { get; set; } = string.Empty;
    public enEntityState Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
