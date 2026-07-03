namespace BudgetManager.Application.Models;

public class UpdateIncomeRequest
{
    public Guid ApplicationUserId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
}
