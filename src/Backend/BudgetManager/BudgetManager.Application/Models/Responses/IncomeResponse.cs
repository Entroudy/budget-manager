namespace BudgetManager.Application.Models;

public class IncomeResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public DateOnly Date { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
