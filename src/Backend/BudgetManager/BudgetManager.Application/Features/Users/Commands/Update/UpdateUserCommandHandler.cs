using BudgetManager.Application.Models;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application;

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

        user.Update(request.FirstName, request.LastName, request.NickName, request.Email);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
