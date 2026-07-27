namespace BudgetManager.Application;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly AppDbContext _context;

    public UpdateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ApplicationUserId);
        if (user == null)
            throw new NotFoundException($"User with {request.ApplicationUserId} dows not exits");

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        user.Update(request.FirstName, request.LastName, request.NickName, request.Email);

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!await _roleManager.RoleExistsAsync(request.Role))
                throw new ValidationException($"Role {request.Role} does not exist");

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    throw new ValidationException($"Unable to update roles for {request.ApplicationUserId}");
            }

            var addResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!addResult.Succeeded)
                throw new ValidationException($"Unable to assign {request.Role} role to {request.ApplicationUserId}");
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
