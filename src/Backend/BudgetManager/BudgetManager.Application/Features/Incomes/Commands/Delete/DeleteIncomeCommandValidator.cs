namespace BudgetManager.Application;

public class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
{
    public DeleteIncomeCommandValidator()
    {
        RuleFor(x => x.IncomeId).NotEmpty();
    }
}
