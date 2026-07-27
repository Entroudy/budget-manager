namespace BudgetManager.Application;

public class GetSpendingByIdQuery : IRequest<SpendingResponse>
{
    public GetSpendingByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
