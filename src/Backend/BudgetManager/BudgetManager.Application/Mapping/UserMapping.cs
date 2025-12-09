using BudgetManager.Application.Models.Responses;
using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Mappings;

public static class UserMapping
{
    public static IQueryable<UserResponse> ToUserResponse(this IQueryable<ApplicationUser> entity)
    {
        return entity.Select(x => new UserResponse
        {
            Id = x.Id,
            Email = x.Email,
            LastName = x.LastName,
            FirstName = x.FirstName,
            NickName = x.NickName
        });
    }
}