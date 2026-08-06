namespace UI.Areas.Admin.Models;

public class ManagementPageViewModel<T>
{
    public string PageTitle { get; set; } = string.Empty;
    public string PageDescription { get; set; } = string.Empty;
    public string AddButtonText { get; set; } = "Add New";
    public string AddButtonController { get; set; } = string.Empty;
    public string RecordLabel { get; set; } = "records";
    public string RecordIcon { get; set; } = "fa-list";
    public string SortedBy { get; set; } = "Name";
    public List<T> Items { get; set; } = new();
    public PaginationModel Pagination { get; set; } = new();
}
