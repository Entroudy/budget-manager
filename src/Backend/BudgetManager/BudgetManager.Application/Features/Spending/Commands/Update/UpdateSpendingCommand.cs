namespace BudgetManager.Application;

public class UpdateSpendingCommand(UpdateSpendingRequest request) : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid? ApplicationUserId { get; set; } = request.ApplicationUserId;
    public string Item { get; set; } = request.Item;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
    public string? Color { get; set; } = request.Color;
    public DateOnly Date { get; set; } = request.Date;
}
