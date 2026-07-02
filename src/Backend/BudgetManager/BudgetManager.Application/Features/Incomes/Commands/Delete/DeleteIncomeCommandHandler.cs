using BudgetManager.Common.Exceptions;
using BudgetManager.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand, Unit>
{
    private readonly AppDbContext _context;

    public DeleteIncomeCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(x => x.Id == request.IncomeId, cancellationToken)
            ?? throw new NotFoundException("Income", request.IncomeId.ToString());

        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

