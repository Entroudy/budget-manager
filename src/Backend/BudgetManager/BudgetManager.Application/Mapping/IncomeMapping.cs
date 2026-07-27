namespace BudgetManager.Application.Mappings;

public static class IncomeMapping
{
    public static IQueryable<IncomeResponse> ToIncomeResponse(this IQueryable<Income> entity)
    {
        return entity
            .OrderByDescending(x => x.Date)
            .Select(x => new IncomeResponse
        {
            Id = x.Id,
            ApplicationUserId = x.ApplicationUserId,
            Amount = x.Amount,
            PaymentMethod = x.PaymentMethod,
            Comment = x.Comment,
            Date = x.Date,
            CreatedDate = x.CreatedAt,
            LastModifiedDate = x.ModifiedAt
        });
    }
}
