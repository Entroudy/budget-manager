using BudgetManager.Common;
using Xunit;

namespace BudgetManager.Tests;

public class AppRolesTests
{
    [Fact]
    public void All_contains_admin_and_user_roles_in_the_expected_order()
    {
        Assert.Equal(new[] { AppRoles.Admin, AppRoles.User }, AppRoles.All);
    }

    [Fact]
    public void All_contains_only_unique_roles()
    {
        Assert.Equal(AppRoles.All.Length, AppRoles.All.Distinct().Count());
    }
}
