using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Models.Responses;

public class SpendingListResponse
{
    public Guid Id { get; set; }
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
