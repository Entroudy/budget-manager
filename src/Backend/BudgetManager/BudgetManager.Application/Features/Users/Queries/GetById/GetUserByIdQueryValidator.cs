namespace BudgetManager.Application;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}
