using System.ComponentModel.DataAnnotations;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application.Features.Users.DefaultUserSeed
{
    public static class DefaultUserInitializer
    {
        public static void Seed(UserManager<ApplicationUser> userManager, AppDbContext context, string defaultUserEmail, string defaultUserPassword)
        {
            if (string.IsNullOrWhiteSpace(defaultUserEmail))
                defaultUserEmail = "admin@entroudy.com";

            if (string.IsNullOrWhiteSpace(defaultUserPassword))
                defaultUserPassword = "Almafa123!";

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(defaultUserEmail))
                throw new ValidationException($"Given super admin email \" {defaultUserEmail} \" is not a valid address");

            if (userManager.FindByEmailAsync(defaultUserEmail).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = defaultUserEmail,
                    Email = defaultUserEmail
                };

                IdentityResult result = userManager.CreateAsync(user, defaultUserPassword).Result;

                if (result.Succeeded)
                {
                    var currentUser = context.ApplicationUsers.FirstOrDefault(x => x.Email == defaultUserEmail);

                    context.SaveChanges();

                }
            }
        }
    }

}
