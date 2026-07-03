namespace BudgetManager.Application;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<UserResponse>>
{
    private readonly AppDbContext _context;

    public GetUserListQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedResult<UserResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        return _context.ApplicationUsers
            .Include(x => x.Incomes)
            .ToUserResponse()
            .Sort(request.PaginationPostModel)
            .Search(request.PaginationPostModel, x => x.Email, x => x.LastName, x => x.FirstName, x => x.NickName)
            .ToPaginatedResultAsync(request.PaginationPostModel);
    }
}
