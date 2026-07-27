namespace BudgetManager.Application;

public class GetIncomesByUserIdQueryHandler : IRequestHandler<GetIncomesByUserIdQuery, List<IncomeResponse>>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetIncomesByUserIdQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<IncomeResponse>> Handle(GetIncomesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var applicationUserId = request.ApplicationUserId;
        _currentUserService.EnsureEligibleForApplicationUser(applicationUserId);

        return await _context.Incomes
            .Where(x => x.ApplicationUserId == applicationUserId)
            .OrderByDescending(x => x.Date)
            .ToIncomeResponse()
            .ToListAsync(cancellationToken);
    }
}
