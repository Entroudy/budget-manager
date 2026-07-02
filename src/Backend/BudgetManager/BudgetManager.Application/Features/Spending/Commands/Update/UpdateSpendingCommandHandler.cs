using BudgetManager.Application;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

public class UpdateSpendingCommandHandler : IRequestHandler<UpdateSpendingCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateSpendingCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateSpendingCommand request, CancellationToken cancellationToken)
    {
        Guid applicationUserId = request.ApplicationUserId ?? Guid.Empty;

        if (request.ApplicationUserId == null && Guid.TryParse(_currentUserService.ApplicationUserId, out var currentUserId))
        {
            applicationUserId = currentUserId;
        }

        var spending = await _context.Spendings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (spending == null)
        {
            throw new ValidationException($"Spending with ID {request.Id} not found");
        }

        spending.Update(applicationUserId, request.Item, request.Amount, request.PaymentMethod, request.Comment, request.Color, request.Date);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
