namespace BudgetManager.Application;

public class UpdateSpendingCommandValidator : AbstractValidator<UpdateSpendingCommand>
{
    public UpdateSpendingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid spending ID");

        RuleFor(x => x.Item)
            .NotEmpty()
            .WithMessage("Item is required")
            .MaximumLength(200)
            .WithMessage("Item must not exceed 200 characters");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment method");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Comment));

        RuleFor(x => x.Date)
           .NotEmpty()
           .WithMessage("Date is required");
    }
}
