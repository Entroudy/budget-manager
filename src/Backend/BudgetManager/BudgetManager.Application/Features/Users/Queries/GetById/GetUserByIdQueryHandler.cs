namespace BudgetManager.Application;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers
            .Include(x => x.Incomes)
            .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException("User", request.Id);

        return await user.ToUserResponseAsync(_userManager);
    }
}
