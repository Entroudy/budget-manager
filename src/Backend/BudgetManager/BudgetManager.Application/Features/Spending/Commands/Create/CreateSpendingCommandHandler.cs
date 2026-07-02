using BudgetManager.Application;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using MediatR;

namespace BudgetManager.Application;

public class CreateSpendingCommandHandler : IRequestHandler<CreateSpendingCommand, Guid>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateSpendingCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateSpendingCommand request, CancellationToken cancellationToken)
    {
        Guid applicationUserId = request.ApplicationUserId ?? Guid.Empty;

        if (request.ApplicationUserId == null && Guid.TryParse(_currentUserService.ApplicationUserId, out var currentUserId))
        {
            applicationUserId = currentUserId;
        }

        var spending = new Spending(applicationUserId, request.Item, request.Amount, request.PaymentMethod, request.Comment, request.Color, request.Date);

        _context.Spendings.Add(spending);
        await _context.SaveChangesAsync(cancellationToken);

        return spending.Id;
    }
}
