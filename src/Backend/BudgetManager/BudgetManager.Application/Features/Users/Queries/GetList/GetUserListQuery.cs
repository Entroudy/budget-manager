namespace BudgetManager.Application;

public class GetUserListQuery : IRequest<PaginatedResult<UserResponse>>
{
    public GetUserListQuery(PaginationPostModel paginationPostModel)
    {
        PaginationPostModel = paginationPostModel;
    }

    public PaginationPostModel PaginationPostModel { get; private set; }
}
