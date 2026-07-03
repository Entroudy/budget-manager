namespace BudgetManager.Application;

public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand, IncomeResponse>
{
    private readonly AppDbContext _context;

    public UpdateIncomeCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IncomeResponse> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(x => x.Id == request.IncomeId, cancellationToken)
            ?? throw new NotFoundException("Income", request.IncomeId.ToString());

        income.Update(request.ApplicationUserId, request.Amount, request.PaymentMethod, request.Comment);

        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Incomes
            .Where(x => x.Id == income.Id)
            .ToIncomeResponse()
            .FirstAsync(cancellationToken);
    }
}
