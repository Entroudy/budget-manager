namespace BudgetManager.Application.Models.Responses;
public class SuccessAuthenticateResponse
{
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}