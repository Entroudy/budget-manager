namespace BudgetManager.Application;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(request.Email);
        if (userExists != null && userExists.Id != null)
            throw new ValidationException($"User with {request.Email} already exits");

        var roleName = string.IsNullOrWhiteSpace(request.Role) ? AppRoles.User : request.Role;
        if (!await _roleManager.RoleExistsAsync(roleName))
            throw new ValidationException($"Role {roleName} does not exist");

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

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
            var createdUser = await _userManager.FindByEmailAsync(request.Email);
            if (createdUser != null)
            {
                var roleResult = await _userManager.AddToRoleAsync(createdUser, roleName);
                if (!roleResult.Succeeded)
                    throw new ValidationException($"Unable to assign {roleName} role to {request.Email}");
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }

        throw new Exception($"Something went wrong at user {request.Email} creation");
    }
}
