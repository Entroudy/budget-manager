using FluentValidation;

namespace BudgetManager.Application;

public class DeleteSpendingCommandValidator : AbstractValidator<DeleteSpendingCommand>
{
    public DeleteSpendingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");
    }
}
