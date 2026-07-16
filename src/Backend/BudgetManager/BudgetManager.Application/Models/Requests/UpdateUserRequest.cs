namespace BudgetManager.Application.Models;

public class UpdateUserRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NickName { get; set; }
    public string? Role { get; set; }

} 
