namespace BudgetManager.Application.Mappings;

public static class UserMapping
{
    public static async Task<UserResponse> ToUserResponseAsync(this ApplicationUser entity, UserManager<ApplicationUser> userManager)
    {
        var roles = (await userManager.GetRolesAsync(entity)).ToList();
        if (!roles.Any())
            roles = new List<string> { AppRoles.User };

        return new UserResponse
        {
            Id = entity.Id,
            Email = entity.Email,
            LastName = entity.LastName,
            FirstName = entity.FirstName,
            NickName = entity.NickName,
            ProfileImage = entity.ProfileImage,
            Roles = roles.ToList(),
            Incomes = entity.Incomes
                .OrderByDescending(i => i.Date)
                .Select(i => new IncomeResponse
                {
                    Id = i.Id,
                    ApplicationUserId = i.ApplicationUserId,
                    Amount = i.Amount,
                    PaymentMethod = i.PaymentMethod,
                    Comment = i.Comment,
                    Date = i.Date,
                    CreatedDate = i.CreatedAt,
                    LastModifiedDate = i.ModifiedAt
                }).ToList()
        };
    }
}
