namespace BudgetManager.Application;

public class GetSpendingByIdQueryHandler : IRequestHandler<GetSpendingByIdQuery, SpendingResponse>
{
    private readonly AppDbContext _context;

    public GetSpendingByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SpendingResponse> Handle(GetSpendingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Spendings
            .ToSpendingResponse()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Spending", request.Id.ToString());
    }
}
