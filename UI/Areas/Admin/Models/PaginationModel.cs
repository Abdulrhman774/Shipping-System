namespace UI.Areas.Admin.Models;

public class PaginationModel
{
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalItems { get; set; }
    public int PageSize { get; set; } = 10;
    public string ItemLabel { get; set; } = "records";
}
