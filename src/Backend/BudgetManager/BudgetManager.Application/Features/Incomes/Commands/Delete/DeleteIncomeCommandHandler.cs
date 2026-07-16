namespace BudgetManager.Application;

public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteIncomeCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(x => x.Id == request.IncomeId, cancellationToken)
            ?? throw new NotFoundException("Income", request.IncomeId.ToString());

        _currentUserService.EnsureEligibleForApplicationUser(income.ApplicationUserId);

        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

