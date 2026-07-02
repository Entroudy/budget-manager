using MediatR;

namespace BudgetManager.Application;

public class DeleteSpendingCommand(Guid id) : IRequest<Unit>
{
    public Guid Id { get; set; } = id;
}
