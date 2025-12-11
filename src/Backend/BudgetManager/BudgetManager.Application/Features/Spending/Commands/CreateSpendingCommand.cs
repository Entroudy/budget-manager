using BudgetManager.Application.Models.Requests;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;

namespace BudgetManager.Application;

public class CreateSpendingCommand(CreateSpendingRequest request) : IRequest<Guid>
{
    public Guid? ApplicationUserId { get; set; } = request.ApplicationUserId;
    public string Item { get; set; } = request.Item;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
    public string? Color { get; set; } = request.Color;
    public DateTime Date { get; set; } = request.Date;
}

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

public class CreateSpendingCommandValidator : AbstractValidator<CreateSpendingCommand>
{
    public CreateSpendingCommandValidator()
    {
        RuleFor(x => x.Item)
            .NotEmpty()
            .WithMessage("Item is required")
            .MaximumLength(200)
            .WithMessage("Item must not exceed 200 characters");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment method");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Comment));

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required");
    }
}
