namespace BudgetManager.Application.Mappings;

public static class UserMapping
{
    public static IQueryable<UserResponse> ToUserResponse(this IQueryable<ApplicationUser> entity)
    {
        return entity.Select(x => new UserResponse
        {
            Id = x.Id,
            Email = x.Email,
            LastName = x.LastName,
            FirstName = x.FirstName,
            NickName = x.NickName,
            Incomes = x.Incomes
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
        });
    }
}
