using BudgetManager.Application.Extensions;
using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Infrastructure;
using MediatR;

namespace BudgetManager.Application;

public class GetSpendingListQueryHandler : IRequestHandler<GetSpendingListQuery, PaginatedResult<SpendingListResponse>>
{
    private readonly AppDbContext _context;

    public GetSpendingListQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedResult<SpendingListResponse>> Handle(GetSpendingListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Spendings.AsQueryable();

        if (request.FilterPostModel.ApplicationUserId.HasValue)
        {
            query = query.Where(x => x.ApplicationUserId == request.FilterPostModel.ApplicationUserId.Value);
        }

        if (request.FilterPostModel.FromDate.HasValue)
        {
            query = query.Where(x => x.Date >= request.FilterPostModel.FromDate.Value);
        }

        if (request.FilterPostModel.ToDate.HasValue)
        {
            query = query.Where(x => x.Date <= request.FilterPostModel.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.FilterPostModel.Color))
        {
            query = query.Where(x => x.Color == request.FilterPostModel.Color);
        }

        query = query.Sort(request.FilterPostModel).Search(request.FilterPostModel, x => x.Item);

        return query.ToSpendingListResponse().ToPaginatedResultAsync(request.FilterPostModel);
    }
}
