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
    public GetSpendingListQuery(PaginationPostModel paginationPostModel)
    {
        PaginationPostModel = paginationPostModel;
    }
    public PaginationPostModel PaginationPostModel { get; private set; }
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
        var query = _context.Spendings.Search(request.PaginationPostModel, x => x.Item);

        return query.ToSpendingListResponse().ToPaginatedResultAsync(request.PaginationPostModel);
    }
}

public class GetSpendingListQueryValidator : AbstractValidator<GetSpendingListQuery>
{
    public GetSpendingListQueryValidator()
    {
        RuleFor(x => x.PaginationPostModel).NotNull();
    }
}
