namespace BudgetManager.Application;

public class GetUserByIdQuery : IRequest<UserResponse>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
