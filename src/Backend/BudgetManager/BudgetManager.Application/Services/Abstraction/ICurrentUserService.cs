namespace BudgetManager.Application;

public interface ICurrentUserService
{
    string? ApplicationUserId { get; }
    bool IsAuthenticated { get; }
}
