using BudgetManager.Application.Models.Responses;
using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Mappings;

public static class SpendingMapping
{
    public static IQueryable<SpendingResponse> ToSpendingResponse(this IQueryable<Spending> entity)
    {
        return entity.Select(s => new SpendingResponse
        {
            Id = s.Id,
            Item = s.Item,
            Amount = s.Amount,
            PaymentMethod = s.PaymentMethod,
            Comment = s.Comment,
            Color = s.Color,
            CreatedAt = s.CreatedAt,
            CreatedBy = s.CreatedBy,
            ModifiedAt = s.ModifiedAt,
            ModifiedBy = s.ModifiedBy
        });
    }

    public static IQueryable<SpendingListResponse> ToSpendingListResponse(this IQueryable<Spending> entity)
    {
        return entity.Select(s => new SpendingListResponse
        {
            Id = s.Id,
            Item = s.Item,
            Amount = s.Amount,
            PaymentMethod = s.PaymentMethod,
            Comment = s.Comment,
            Color = s.Color,
            CreatedAt = s.CreatedAt,
            CreatedBy = s.CreatedBy
        });
    }
}
