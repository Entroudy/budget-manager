namespace BudgetManager.Application;

public class UserAuthenticateCommandValidator : AbstractValidator<UserAuthenticateCommand>
{
    public UserAuthenticateCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
