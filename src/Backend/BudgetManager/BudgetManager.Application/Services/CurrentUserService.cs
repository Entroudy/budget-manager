using BudgetManager.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BudgetManager.Application;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    public string? ApplicationUserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid)
        ?? _httpContextAccessor.HttpContext?.Items["ApplicationUserId"]?.ToString();
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public bool IsInRole(string role) =>
        _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    public bool HasAdminPrivileges => IsInRole(AppRoles.Admin);

    public Guid GetRequiredApplicationUserId()
    {
        if (!Guid.TryParse(ApplicationUserId, out var applicationUserId))
            throw new UnauthorizedAccessException();

        return applicationUserId;
    }

    public bool IsEligibleForApplicationUser(Guid applicationUserId)
    {
        if (HasAdminPrivileges)
            return true;

        if (!Guid.TryParse(ApplicationUserId, out var currentUserId))
            return false;

        return currentUserId == applicationUserId;
    }

    public void EnsureEligibleForApplicationUser(Guid applicationUserId)
    {
        if (!IsEligibleForApplicationUser(applicationUserId))
            throw new ForbiddenException("You do not have access to this user");
    }

    public Guid ResolveApplicationUserIdForCreate(Guid? requestedApplicationUserId = null)
    {
        var currentUserId = GetRequiredApplicationUserId();

        if (requestedApplicationUserId.HasValue && requestedApplicationUserId.Value != Guid.Empty)
        {
            if (HasAdminPrivileges || requestedApplicationUserId.Value == currentUserId)
                return requestedApplicationUserId.Value;

            throw new ForbiddenException("You can only create items for your own account");
        }

        return currentUserId;
    }

    public Guid ResolveApplicationUserIdForUpdate(Guid currentApplicationUserId, Guid? requestedApplicationUserId = null)
    {
        if (HasAdminPrivileges)
            return requestedApplicationUserId.GetValueOrDefault(currentApplicationUserId);

        EnsureEligibleForApplicationUser(currentApplicationUserId);
        return currentApplicationUserId;
    }
}
