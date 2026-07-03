namespace BudgetManager.Application;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly AppDbContext _context;

    public GetUserByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ApplicationUsers
            .Include(x => x.Incomes)
            .ToUserResponse()
            .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException("User", request.Id);
    }
}
