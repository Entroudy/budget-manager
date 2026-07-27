namespace BudgetManager.Application;

public class GetIncomesByUserIdQuery : IRequest<List<IncomeResponse>>
{
    public GetIncomesByUserIdQuery(Guid applicationUserId)
    {
        ApplicationUserId = applicationUserId;
    }

    public Guid ApplicationUserId { get; private set; }
}
