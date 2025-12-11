using BudgetManager.Application.Models;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application.ApplicationUsers;
public class UpdateUserCommand(string id, UpdateUserRequest request) : IRequest<Unit>
{
    public string ApplicationUserId { get; set; } = id;
    public string Email { get; set; } = request.Email;
    public string FirstName { get; set; } = request.FirstName;
    public string LastName { get; set; } = request.LastName;
    public string NickName { get; set; } = request.NickName;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ApplicationUserId);
        if (user == null)
            throw new NotFoundException($"User with {request.ApplicationUserId} dows not exits");

        user.Update(request.FirstName, request.LastName, request.NickName);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.ApplicationUserId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.NickName).NotEmpty();
    }
}