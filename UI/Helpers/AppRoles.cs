namespace UI.Helpers;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string OpManager = "OpManager";
    public const string Reviewer = "Reviewer";
    public const string Op = "Op";
    public const string User = "User";

    // Convenience groups
    public const string AllAdminRoles = $"{Admin},{OpManager},{Reviewer},{Op}";
    public const string CanApprove = $"{Admin},{OpManager},{Reviewer}";
    public const string CanMarkReadyForShip = $"{Admin},{OpManager},{Op}";
    public const string CanMarkShipped = $"{Admin},{OpManager}";
    public const string CanCreateEdit = $"{Admin},{Op}";
    public const string CanDelete = Admin;
    public const string CanViewAnalytics = $"{Admin},{OpManager},{Reviewer}";
}