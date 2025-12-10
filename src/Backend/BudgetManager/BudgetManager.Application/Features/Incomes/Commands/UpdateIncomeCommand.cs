using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Requests;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Incomes;

public class UpdateIncomeCommand(Guid incomeId, UpdateIncomeRequest request) : IRequest<IncomeResponse>
{
    public Guid IncomeId { get; set; } = incomeId;
    public Guid ApplicationUserId { get; set; } = request.ApplicationUserId;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
}

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

public class UpdateIncomeCommandValidator : AbstractValidator<UpdateIncomeCommand>
{
    public UpdateIncomeCommandValidator()
    {
        RuleFor(x => x.IncomeId).NotEmpty();
        RuleFor(x => x.ApplicationUserId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentMethod).IsInEnum();
    }
}
