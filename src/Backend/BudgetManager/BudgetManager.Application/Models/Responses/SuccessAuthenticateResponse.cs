namespace BudgetManager.Application.Models;

public class SuccessAuthenticateResponse
{
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}