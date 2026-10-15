using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser Update(string? firstName, string? lastName, string? nickName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        NickName = nickName;
        Email = email;

        return this;
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NickName { get; set; }
    public string? ProfileImage { get; set; }
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Spending> Spendings { get; set; } = new List<Spending>();

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
