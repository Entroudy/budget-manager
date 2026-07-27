namespace BudgetManager.Application.Models;

public class LoginRequest
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public int TokenExpiration { get; set; }
}

