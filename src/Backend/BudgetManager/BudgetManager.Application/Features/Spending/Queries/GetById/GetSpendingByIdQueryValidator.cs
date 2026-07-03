namespace BudgetManager.Application;

public class GetSpendingByIdQueryValidator : AbstractValidator<GetSpendingByIdQuery>
{
    public GetSpendingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");
    }
}
