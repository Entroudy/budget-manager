namespace BudgetManager.Application;

public class GetSpendingByIdQueryHandler : IRequestHandler<GetSpendingByIdQuery, SpendingResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSpendingByIdQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<SpendingResponse> Handle(GetSpendingByIdQuery request, CancellationToken cancellationToken)
    {
        var spending = await _context.Spendings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Spending", request.Id.ToString());

        _currentUserService.EnsureEligibleForApplicationUser(spending.ApplicationUserId);

        return await _context.Spendings
            .Where(x => x.Id == request.Id)
            .ToSpendingResponse()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Spending", request.Id.ToString());
    }
}
