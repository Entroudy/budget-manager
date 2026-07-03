namespace BudgetManager.Application;

public class UserAuthenticateCommandHandler : IRequestHandler<UserAuthenticateCommand, SuccessAuthenticateResponse>
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserAuthenticateCommandHandler(
        AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<SuccessAuthenticateResponse> Handle(UserAuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
            throw new ValidationException("User does not exits");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
            throw new ValidationException("Bad Password or Email");

        if (request.TokenExpiration > 2160)
            throw new ValidationException("The token's lifetime must be under 2160 hours!");

        var claims = new Dictionary<string, object>
        {
            { "name", user.UserName },
            { ClaimTypes.Sid, user.Id },
        };

        var token = getToken(claims, request.TokenExpiration);

        return new SuccessAuthenticateResponse()
        {
            Token = token.RawData,
            ValidTo = token.ValidTo,
        };
    }

    private JwtSecurityToken getToken(Dictionary<string, object> authClaims, int tokenExpiration)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT_Secret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AppSettings.JWT_ValidIssuer,
            Audience = AppSettings.JWT_ValidAudience,
            Claims = authClaims,
            Expires = DateTime.UtcNow.AddHours(tokenExpiration),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        return new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
    }
}
