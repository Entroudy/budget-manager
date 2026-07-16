using BudgetManager.Application.Exceptions;

namespace BudgetManager.Application;

public class GetSpendingListQueryHandler : IRequestHandler<GetSpendingListQuery, PaginatedResult<SpendingListResponse>>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSpendingListQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public Task<PaginatedResult<SpendingListResponse>> Handle(GetSpendingListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Spendings.AsQueryable();

        if (_currentUserService.HasAdminPrivileges)
        {
            if (request.FilterPostModel.ApplicationUserId.HasValue)
            {
                query = query.Where(x => x.ApplicationUserId == request.FilterPostModel.ApplicationUserId.Value);
            }
        }
        else
        {
            var currentUserId = _currentUserService.GetRequiredApplicationUserId();

            if (request.FilterPostModel.ApplicationUserId.HasValue && request.FilterPostModel.ApplicationUserId.Value != currentUserId)
                throw new ForbiddenException("You can only access your own spendings");

            query = query.Where(x => x.ApplicationUserId == currentUserId);
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
