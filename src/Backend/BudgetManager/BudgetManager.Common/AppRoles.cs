namespace BudgetManager.Common;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string User = "User";

    public static readonly string[] All = new[] { Admin, User };
}
