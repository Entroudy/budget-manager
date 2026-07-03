namespace BudgetManager.Application.Models;

public class SpendingListResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserNickName { get; set; } = string.Empty;
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
