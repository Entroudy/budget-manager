using BudgetManager.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application.Users;
public class DeleteUserCommand : IRequest<Unit>
{
    public DeleteUserCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null || user.Id == null)
            throw new ValidationException($"User with {request.Id} id not exits");

        await _userManager.DeleteAsync(user);

        return Unit.Value;
    }
}
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}