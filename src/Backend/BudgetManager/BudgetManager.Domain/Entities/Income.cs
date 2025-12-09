namespace BudgetManager.Domain.Entities;

public class Income : AuditableEntity
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }

    public Income()
    {
    }
}
