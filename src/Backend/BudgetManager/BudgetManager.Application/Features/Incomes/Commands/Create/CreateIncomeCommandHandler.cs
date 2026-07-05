namespace BudgetManager.Application;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, IncomeResponse>
{
    private readonly AppDbContext _context;

    public CreateIncomeCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IncomeResponse> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = new Income(request.ApplicationUserId, request.Amount, request.PaymentMethod, request.Comment, request.Date);

        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Incomes
            .Where(x => x.Id == income.Id)
            .ToIncomeResponse()
            .FirstAsync(cancellationToken);
    }
}
