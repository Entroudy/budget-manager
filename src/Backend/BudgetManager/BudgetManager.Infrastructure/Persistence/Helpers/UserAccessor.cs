using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BudgetManager.Infrastructure.Persistence.Helpers
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.Items["ApplicationUserId"].ToString();
            Guid result;

            if (userIdString != null)
            {
                if (!Guid.TryParse(userIdString, out result))
                    throw new InvalidOperationException("Unable to receive userid from the user");
            }
            else
            {
                result = Guid.Empty;
            }

            return result;
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.Items["UserName"].ToString();
        }
    }
}
