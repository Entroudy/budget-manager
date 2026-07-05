namespace BudgetManager.Application.Models;

public class UpdateSpendingRequest
{
    public Guid? ApplicationUserId { get; set; }
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateOnly Date { get; set; }
}
