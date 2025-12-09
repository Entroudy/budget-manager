using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser Update(string? firstName, string? lastName, string? nickName)
    {
        FirstName = firstName;
        LastName = lastName;
        NickName = nickName;

        return this;
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NickName { get; set; }
    public ICollection<Income> Incomes { get; set; }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
