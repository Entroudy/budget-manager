using System.Text;
using BudgetManager.Application.Models;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using FluentValidation;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application.Users;
public class ChangePasswordCommand(string id, ChangePasswordRequest request) : IRequest<Unit>
{
    public string UserId { get; set; } = id;
    public string Password { get; set; } = request.Password;
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException($"User with {request.UserId} dows not exits");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (result.Errors.Any())
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Validation error occurred at password reset user: {user.Email}. Errors: ");
            result.Errors.ForEach(x => stringBuilder.AppendLine($"{x.Description} "));

            throw new FluentValidation.ValidationException(stringBuilder.ToString());
        }

        if (result.Succeeded)
            return Unit.Value;

        throw new Exception("Can't change users password");
    }
}
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}