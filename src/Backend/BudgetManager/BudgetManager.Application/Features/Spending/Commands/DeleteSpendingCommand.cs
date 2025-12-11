using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

public class DeleteSpendingCommand(Guid id) : IRequest<Unit>
{
    public Guid Id { get; set; } = id;
}

public class DeleteSpendingCommandHandler : IRequestHandler<DeleteSpendingCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteSpendingCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteSpendingCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new ValidationException("Unauthorized");

        if (!Guid.TryParse(_currentUserService.ApplicationUserId, out var currentUserId))
        {
            throw new ValidationException("User not found");
        }

        var spending = await _context.Spendings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (spending == null)
        {
            throw new ValidationException($"Spending with ID {request.Id} not found");
        }

        spending.Delete(currentUserId.ToString());

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteSpendingCommandValidator : AbstractValidator<DeleteSpendingCommand>
{
    public DeleteSpendingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");
    }
}
