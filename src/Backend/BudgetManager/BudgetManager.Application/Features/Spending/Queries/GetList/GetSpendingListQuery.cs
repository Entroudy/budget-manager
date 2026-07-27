namespace BudgetManager.Application;

public class GetSpendingListQuery : IRequest<PaginatedResult<SpendingListResponse>>
{
    public GetSpendingListQuery(SpendingFilterPostModel filterPostModel)
    {
        FilterPostModel = filterPostModel;
    }

    public SpendingFilterPostModel FilterPostModel { get; private set; }
}
