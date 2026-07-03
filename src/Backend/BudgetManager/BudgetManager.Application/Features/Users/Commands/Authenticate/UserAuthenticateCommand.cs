namespace BudgetManager.Application;

public class UserAuthenticateCommand : IRequest<SuccessAuthenticateResponse>
{
    public UserAuthenticateCommand(string email, string password, int tokenExpiration)
    {
        Email = email;
        Password = password;
        TokenExpiration = tokenExpiration;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public int TokenExpiration { get; set; }
}
