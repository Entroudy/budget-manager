namespace BudgetManager.Application;

public class GetSpendingListQueryValidator : AbstractValidator<GetSpendingListQuery>
{
    public GetSpendingListQueryValidator()
    {
        RuleFor(x => x.FilterPostModel).NotNull();
    }
}
