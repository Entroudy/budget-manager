using MediatR;

namespace BudgetManager.Application;

public class DeleteIncomeCommand : IRequest<Unit>
{
    public DeleteIncomeCommand(Guid incomeId)
    {
        IncomeId = incomeId;
    }

    public Guid IncomeId { get; set; }
}
