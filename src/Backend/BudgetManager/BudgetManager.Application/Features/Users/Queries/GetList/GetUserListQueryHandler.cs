namespace BudgetManager.Application;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<UserResponse>>
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserListQueryHandler(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<PaginatedResult<UserResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var pagedUsers = await _context.ApplicationUsers
            .Include(x => x.Incomes)
            .Sort(request.PaginationPostModel)
            .Search(request.PaginationPostModel, x => x.Email, x => x.LastName, x => x.FirstName, x => x.NickName)
            .ToPaginatedResultAsync(request.PaginationPostModel);

        var responses = new List<UserResponse>();
        foreach (var user in pagedUsers.Results)
        {
            responses.Add(await user.ToUserResponseAsync(_userManager));
        }

        return new PaginatedResult<UserResponse>(
            pagedUsers.TotalCount,
            pagedUsers.TotalPage,
            pagedUsers.CurrentPage,
            responses);
    }
}
