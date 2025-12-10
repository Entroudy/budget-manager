using BudgetManager.Application.Models.Responses;
using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Mappings;

public static class IncomeMapping
{
    public static IQueryable<IncomeResponse> ToIncomeResponse(this IQueryable<Income> entity)
    {
        return entity.Select(x => new IncomeResponse
        {
            Id = x.Id,
            ApplicationUserId = x.ApplicationUserId,
            Amount = x.Amount,
            PaymentMethod = x.PaymentMethod,
            Comment = x.Comment,
            CreatedDate = x.CreatedAt,
            LastModifiedDate = x.ModifiedAt
        });
    }
}
