namespace BudgetManager.Application;

public class CreateSpendingCommand(CreateSpendingRequest request) : IRequest<Guid>
{
    public Guid? ApplicationUserId { get; set; } = request.ApplicationUserId;
    public string Item { get; set; } = request.Item;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
    public string? Color { get; set; } = request.Color;
    public DateTime Date { get; set; } = request.Date;
}
