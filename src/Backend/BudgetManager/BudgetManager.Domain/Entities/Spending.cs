namespace BudgetManager.Domain.Entities;

public class Spending : AuditableEntity
{
    public Guid Id { get; set; }
    public string Item { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public string? Color { get; set; }
    public DateOnly Date { get; set; }

    public Spending(string item, decimal amount, PaymentMethod paymentMethod, string? comment, string? color, DateOnly date)
    {
        Item = item;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Comment = comment;
        Color = color;
        Date = date;
    }

    public Spending Update(string item, decimal amount, PaymentMethod paymentMethod, string? comment, string? color, DateOnly date)
    {
        Item = item;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Comment = comment;
        Color = color;
        Date = date;

        return this;
    }
}

public enum PaymentMethod
{
    Cash,
    Card
}
