namespace BudgetManager.Infrastructure.Persistence.Helpers
{
    public class DummyUserAccessor : IUserAccessor
    {
        public Guid GetUserId()
        {
            return Guid.Empty;
        }
        public string GetUserName()
        {
            return "username";
        }
    }
}
