namespace BudgetManager.Application;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, IncomeResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateIncomeCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IncomeResponse> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var applicationUserId = _currentUserService.ResolveApplicationUserIdForCreate(request.ApplicationUserId);

        var income = new Income(applicationUserId, request.Amount, request.PaymentMethod, request.Comment, request.Date);

        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Incomes
            .Where(x => x.Id == income.Id)
            .ToIncomeResponse()
            .FirstAsync(cancellationToken);
    }
}
