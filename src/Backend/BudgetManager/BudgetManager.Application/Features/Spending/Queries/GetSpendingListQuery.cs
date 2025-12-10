using BudgetManager.Application.Extensions;
using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;

namespace BudgetManager.Application;

public class GetSpendingListQuery : IRequest<PaginatedResult<SpendingListResponse>>
{
    public GetSpendingListQuery(SpendingFilterPostModel filterPostModel)
    {
        FilterPostModel = filterPostModel;
    }
    public SpendingFilterPostModel FilterPostModel { get; private set; }
}

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

        // Apply filters
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

        // Apply search and pagination
        query = query.Search(request.FilterPostModel, x => x.Item);

        return query.ToSpendingListResponse().ToPaginatedResultAsync(request.FilterPostModel);
    }
}

public class GetSpendingListQueryValidator : AbstractValidator<GetSpendingListQuery>
{
    public GetSpendingListQueryValidator()
    {
        RuleFor(x => x.FilterPostModel).NotNull();
    }
}
