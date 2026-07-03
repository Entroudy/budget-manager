namespace BudgetManager.Application;

public class GetUserListQueryValidator : AbstractValidator<GetUserListQuery>
{
    public GetUserListQueryValidator()
    {
        RuleFor(x => x.PaginationPostModel).NotNull();
    }
}
