using BudgetManager.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetManager.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string[] _allowedRoles;
    public JwtAuthorizeAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var principal = context.HttpContext.User;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException();
        }

        if (_allowedRoles.Length > 0 && !_allowedRoles.Any(principal.IsInRole))
        {
            throw new ForbiddenException("You do not have access to this resource");
        }
    }
}
