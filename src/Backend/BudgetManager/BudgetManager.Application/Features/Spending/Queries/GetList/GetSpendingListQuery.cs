using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Responses;
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
