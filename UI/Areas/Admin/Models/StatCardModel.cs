namespace UI.Areas.Admin.Models;

public class StatCardModel
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? ChangeValue { get; set; }
    public bool IsPositiveChange { get; set; } = true;
}
