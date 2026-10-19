namespace BudgetManager.Application.Models;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string NickName { get; set; }
    public string? ProfileImage { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<IncomeResponse> Incomes { get; set; } = new();
}
