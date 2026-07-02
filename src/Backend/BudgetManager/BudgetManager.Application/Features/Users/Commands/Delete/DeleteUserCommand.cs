using MediatR;

namespace BudgetManager.Application;

public class DeleteUserCommand : IRequest<Unit>
{
    public DeleteUserCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}
