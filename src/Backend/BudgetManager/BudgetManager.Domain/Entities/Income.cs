namespace BudgetManager.Domain.Entities;

public class Income : AuditableEntity
{
    public Income(Guid applicationUserId, decimal amount, PaymentMethod paymentMethod, string? comment, DateOnly date)
    {
        ApplicationUserId = applicationUserId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Comment = comment;
        Date = date;
    }

    public Income Update(Guid applicationUserId, decimal amount, PaymentMethod paymentMethod, string? comment, DateOnly date)
    {
        ApplicationUserId = applicationUserId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Comment = comment;
        Date = date;

        return this;
    }

    public Guid Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public DateOnly Date { get; set; }
}
