using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Requests;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Incomes;

public class CreateIncomeCommand(CreateIncomeRequest request) : IRequest<IncomeResponse>
{
    public Guid ApplicationUserId { get; set; } = request.ApplicationUserId;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
}

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, IncomeResponse>
{
    private readonly AppDbContext _context;

    public CreateIncomeCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IncomeResponse> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = new Income(request.ApplicationUserId, request.Amount, request.PaymentMethod, request.Comment);

        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Incomes
            .Where(x => x.Id == income.Id)
            .ToIncomeResponse()
            .FirstAsync(cancellationToken);
    }
}

public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
        RuleFor(x => x.ApplicationUserId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentMethod).IsInEnum();
    }
}
