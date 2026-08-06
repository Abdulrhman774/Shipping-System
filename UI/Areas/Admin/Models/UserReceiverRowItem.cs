namespace UI.Areas.Admin.Models;

public class UserReceiverRowItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string Status { get; set; } = string.Empty;
}
