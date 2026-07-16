namespace BudgetManager.Application;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<string>>
{
    public Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(AppRoles.All.ToList());
    }
}
