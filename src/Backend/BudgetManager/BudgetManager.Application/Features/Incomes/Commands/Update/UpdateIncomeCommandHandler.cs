namespace BudgetManager.Application;

public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand, IncomeResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateIncomeCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IncomeResponse> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(x => x.Id == request.IncomeId, cancellationToken)
            ?? throw new NotFoundException("Income", request.IncomeId.ToString());

        var applicationUserId = _currentUserService.ResolveApplicationUserIdForUpdate(income.ApplicationUserId, request.ApplicationUserId);

        income.Update(applicationUserId, request.Amount, request.PaymentMethod, request.Comment, request.Date);

        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Incomes
            .Where(x => x.Id == income.Id)
            .ToIncomeResponse()
            .FirstAsync(cancellationToken);
    }
}
