using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Common.Exceptions;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

public class GetSpendingByIdQuery : IRequest<SpendingResponse>
{
    public GetSpendingByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}

public class GetSpendingByIdQueryHandler : IRequestHandler<GetSpendingByIdQuery, SpendingResponse>
{
    private readonly AppDbContext _context;

    public GetSpendingByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SpendingResponse> Handle(GetSpendingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Spendings
            .ToSpendingResponse()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Spending", request.Id.ToString());
    }
}

public class GetSpendingByIdQueryValidator : AbstractValidator<GetSpendingByIdQuery>
{
    public GetSpendingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");
    }
}
