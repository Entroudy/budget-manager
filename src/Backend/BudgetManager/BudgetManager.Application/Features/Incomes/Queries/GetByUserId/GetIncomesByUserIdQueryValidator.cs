using FluentValidation;

namespace BudgetManager.Application;

public class GetIncomesByUserIdQueryValidator : AbstractValidator<GetIncomesByUserIdQuery>
{
    public GetIncomesByUserIdQueryValidator()
    {
        RuleFor(x => x.ApplicationUserId).NotEmpty();
    }
}
