using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application;

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
            .Where(x => x.ApplicationUserId == request.ApplicationUserId)
            .ToIncomeResponse()
            .ToListAsync(cancellationToken);
    }
}
