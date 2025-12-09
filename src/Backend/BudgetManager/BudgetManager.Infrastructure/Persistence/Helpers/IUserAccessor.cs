namespace BudgetManager.Infrastructure.Persistence.Helpers
{
    public interface IUserAccessor
    {
        Guid GetUserId();
        string GetUserName();
    }
}