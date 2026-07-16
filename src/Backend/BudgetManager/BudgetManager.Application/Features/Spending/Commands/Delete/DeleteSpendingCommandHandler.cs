namespace BudgetManager.Application;

public class DeleteSpendingCommandHandler : IRequestHandler<DeleteSpendingCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteSpendingCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteSpendingCommand request, CancellationToken cancellationToken)
    {
        var spending = await _context.Spendings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (spending == null)
        {
            throw new ValidationException($"Spending with ID {request.Id} not found");
        }

        _currentUserService.EnsureEligibleForApplicationUser(spending.ApplicationUserId);
        spending.Delete(_currentUserService.GetRequiredApplicationUserId().ToString());

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
