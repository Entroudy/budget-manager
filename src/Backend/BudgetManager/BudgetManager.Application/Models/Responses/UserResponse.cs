namespace BudgetManager.Application.Models.Responses;
public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string NickName { get; set; }
    public List<IncomeResponse> Incomes { get; set; } = new();
}