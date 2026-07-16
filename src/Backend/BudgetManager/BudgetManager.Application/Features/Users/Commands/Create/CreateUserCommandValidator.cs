namespace BudgetManager.Application;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.Role)
            .Must(role => string.IsNullOrWhiteSpace(role) || AppRoles.All.Contains(role))
            .WithMessage("Invalid role selected");
    }
}
