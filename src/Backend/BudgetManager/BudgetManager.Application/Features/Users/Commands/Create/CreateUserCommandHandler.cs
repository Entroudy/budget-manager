namespace BudgetManager.Application;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(request.Email);
        if (userExists != null && userExists.Id != null)
            throw new ValidationException($"User with {request.Email} already exits");

        var result = await _userManager.CreateAsync(new ApplicationUser()
        {
            Email = request.Email,
            EmailConfirmed = true,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NickName = request.NickName
        }, request.Password);

        if (result.Errors.Any())
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Validation error occurred at create user. Errors: ");
            result.Errors.ToList().ForEach(x => stringBuilder.AppendLine($"{x.Description} "));

            throw new ValidationException(stringBuilder.ToString());
        }
        else if (result.Succeeded)
        {
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        throw new Exception($"Something went wrong at user {request.Email} creation");
    }
}
