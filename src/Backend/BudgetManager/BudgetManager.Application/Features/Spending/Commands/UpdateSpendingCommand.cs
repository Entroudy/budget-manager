using BudgetManager.Application.Models.Requests;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

public class UpdateSpendingCommand(UpdateSpendingRequest request) : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Item { get; set; } = request.Item;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
    public string? Color { get; set; } = request.Color;
    public DateOnly Date { get; set; } = request.Date;
}

public class UpdateSpendingCommandHandler : IRequestHandler<UpdateSpendingCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateSpendingCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSpendingCommand request, CancellationToken cancellationToken)
    {
        var spending = await _context.Spendings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (spending == null)
        {
            throw new ValidationException($"Spending with ID {request.Id} not found");
        }

        spending.Update(request.Item, request.Amount, request.PaymentMethod, request.Comment, request.Color, request.Date);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateSpendingCommandValidator : AbstractValidator<UpdateSpendingCommand>
{
    public UpdateSpendingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");

        RuleFor(x => x.Item)
            .NotEmpty()
            .WithMessage("Item is required")
            .MaximumLength(200)
            .WithMessage("Item must not exceed 200 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

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
