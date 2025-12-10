using BudgetManager.Common.Exceptions;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Incomes;

public class DeleteIncomeCommand : IRequest<Unit>
{
    public DeleteIncomeCommand(Guid incomeId)
    {
        IncomeId = incomeId;
    }

    public Guid IncomeId { get; set; }
}

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

public class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
{
    public DeleteIncomeCommandValidator()
    {
        RuleFor(x => x.IncomeId).NotEmpty();
    }
}
