namespace BudgetManager.Application;

public class CreateSpendingCommandHandler : IRequestHandler<CreateSpendingCommand, Guid>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateSpendingCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateSpendingCommand request, CancellationToken cancellationToken)
    {
        var applicationUserId = _currentUserService.ResolveApplicationUserIdForCreate(request.ApplicationUserId);

        var spending = new Spending(applicationUserId, request.Item, request.Amount, request.PaymentMethod, request.Comment, request.Color, request.Date);

        _context.Spendings.Add(spending);
        await _context.SaveChangesAsync(cancellationToken);

        return spending.Id;
    }
}
