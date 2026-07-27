
namespace BudgetManager.Application.Features.Users.DefaultUserSeed;

public static class DefaultUserInitializer
{
    public static void Seed(
        UserManager<ApplicationUser> userManager,
        AppDbContext context,
        string defaultUserEmail,
        string defaultUserPassword)
    {
        if (string.IsNullOrWhiteSpace(defaultUserEmail) || string.IsNullOrWhiteSpace(defaultUserPassword))
            return;

        var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (!emailValidator.IsValid(defaultUserEmail))
            throw new ValidationException($"Given super admin email \" {defaultUserEmail} \" is not a valid address");

        if (userManager.FindByEmailAsync(defaultUserEmail).Result == null)
        {
            var user = new ApplicationUser
            {
                UserName = defaultUserEmail,
                Email = defaultUserEmail
            };

            var result = userManager.CreateAsync(user, defaultUserPassword).Result;

            if (result.Succeeded)
            {
                var roleResult = userManager.AddToRoleAsync(user, AppRoles.Admin).Result;
                if (!roleResult.Succeeded)
                    throw new ValidationException($"Unable to assign {AppRoles.Admin} role to {defaultUserEmail}");
                context.SaveChanges();
            }
        }
        else
        {
            var user = userManager.FindByEmailAsync(defaultUserEmail).Result;
            if (user != null && !userManager.IsInRoleAsync(user, AppRoles.Admin).Result)
            {
                var roleResult = userManager.AddToRoleAsync(user, AppRoles.Admin).Result;
                if (!roleResult.Succeeded)
                    throw new ValidationException($"Unable to assign {AppRoles.Admin} role to {defaultUserEmail}");
            }
        }
    }
}
