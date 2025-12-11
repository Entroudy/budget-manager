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
        var user = (string)context.HttpContext.Items["ApplicationUserId"];
        if (user == null)
        {
            // not logged in
            throw new UnauthorizedAccessException();
        }
    }
}