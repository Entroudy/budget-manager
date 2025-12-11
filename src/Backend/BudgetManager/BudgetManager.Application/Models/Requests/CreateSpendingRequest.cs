using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Models.Requests;

public class CreateSpendingRequest
{
    public Guid? ApplicationUserId { get; set; }
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateTime Date { get; set; }
}
