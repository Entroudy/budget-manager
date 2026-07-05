namespace BudgetManager.Application;

public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
        RuleFor(x => x.ApplicationUserId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentMethod).IsInEnum();
        RuleFor(x => x.Date).NotEmpty();
    }
}
