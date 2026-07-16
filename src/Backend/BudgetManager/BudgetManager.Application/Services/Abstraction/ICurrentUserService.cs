namespace BudgetManager.Application;

public interface ICurrentUserService
{
    string? ApplicationUserId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    bool HasAdminPrivileges { get; }
    Guid GetRequiredApplicationUserId();
    bool IsEligibleForApplicationUser(Guid applicationUserId);
    void EnsureEligibleForApplicationUser(Guid applicationUserId);
    Guid ResolveApplicationUserIdForCreate(Guid? requestedApplicationUserId = null);
    Guid ResolveApplicationUserIdForUpdate(Guid currentApplicationUserId, Guid? requestedApplicationUserId = null);
}
