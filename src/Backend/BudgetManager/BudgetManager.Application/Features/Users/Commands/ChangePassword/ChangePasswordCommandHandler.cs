namespace BudgetManager.Application;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ApplicationUserId);
        if (user == null)
            throw new NotFoundException($"User with {request.ApplicationUserId} does not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (result.Errors.Any())
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Validation error occurred while resetting password for user: {user.Email}. Errors:");
            result.Errors.ToList().ForEach(x => stringBuilder.AppendLine($"{x.Description} "));

            throw new FluentValidation.ValidationException(stringBuilder.ToString());
        }

        if (result.Succeeded)
            return Unit.Value;

        throw new Exception("Can't change user password");
    }
}
