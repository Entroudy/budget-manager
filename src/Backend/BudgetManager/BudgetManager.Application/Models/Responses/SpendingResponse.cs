namespace BudgetManager.Application.Models;

public class SpendingResponse
{
    public Guid Id { get; set; }
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
