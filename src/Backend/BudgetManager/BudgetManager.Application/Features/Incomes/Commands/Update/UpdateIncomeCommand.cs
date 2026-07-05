namespace BudgetManager.Application;

public class UpdateIncomeCommand(Guid incomeId, UpdateIncomeRequest request) : IRequest<IncomeResponse>
{
    public Guid IncomeId { get; set; } = incomeId;
    public Guid ApplicationUserId { get; set; } = request.ApplicationUserId;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
    public DateOnly Date { get; set; } = request.Date;
}
