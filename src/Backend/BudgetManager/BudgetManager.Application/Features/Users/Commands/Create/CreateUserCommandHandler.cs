namespace BudgetManager.Application;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(request.Email);
        if (userExists != null && userExists.Id != null)
            throw new ValidationException($"User with {request.Email} already exits");

        var roleName = string.IsNullOrWhiteSpace(request.Role) ? AppRoles.User : request.Role;
        if (!await _roleManager.RoleExistsAsync(roleName))
            throw new ValidationException($"Role {roleName} does not exist");

        var newUser = new ApplicationUser()
        {
            Email = request.Email,
            EmailConfirmed = true,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NickName = request.NickName
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (result.Errors.Any())
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Validation error occurred at create user. Errors: ");
            result.Errors.ToList().ForEach(x => stringBuilder.AppendLine($"{x.Description} "));

            throw new ValidationException(stringBuilder.ToString());
        }
        else if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(newUser, roleName);
                if (!roleResult.Succeeded)
                    throw new ValidationException($"Unable to assign {roleName} role to {request.Email}");
            }

            return Unit.Value;
        }

        throw new Exception($"Something went wrong at user {request.Email} creation");
    }
}
