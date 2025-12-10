using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Incomes;

public class GetIncomesByUserIdQuery : IRequest<List<IncomeResponse>>
{
    public GetIncomesByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
}

public class GetIncomesByUserIdQueryHandler : IRequestHandler<GetIncomesByUserIdQuery, List<IncomeResponse>>
{
    private readonly AppDbContext _context;

    public GetIncomesByUserIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<IncomeResponse>> Handle(GetIncomesByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Incomes
            .Where(x => x.ApplicationUserId == request.UserId)
            .ToIncomeResponse()
            .ToListAsync(cancellationToken);
    }
}

public class GetIncomesByUserIdQueryValidator : AbstractValidator<GetIncomesByUserIdQuery>
{
    public GetIncomesByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
