namespace BudgetManager.Application.Features.Users.DefaultUserSeed;

public static class DefaultRoleInitializer
{
    public static void Seed(RoleManager<IdentityRole<Guid>> roleManager)
    {
        EnsureRole(roleManager, AppRoles.Admin);
        EnsureRole(roleManager, AppRoles.User);
    }

    private static void EnsureRole(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
    {
        if (roleManager.RoleExistsAsync(roleName).Result)
            return;

        var result = roleManager.CreateAsync(new IdentityRole<Guid>(roleName)).Result;
        if (!result.Succeeded)
            throw new ValidationException($"Unable to seed role {roleName}");
    }
}
