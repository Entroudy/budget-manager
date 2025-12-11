using System.Text;
using BudgetManager.Application.Models;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using FluentValidation;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Application.ApplicationUsers;
public class CreateUserCommand(CreateUserRequest request) : IRequest<Unit>
{
    public string Email { get; set; } = request.Email;
    public string Password { get; set; } = request.Password;
    public string FirstName { get; set; } = request.FirstName;
    public string LastName { get; set; } = request.LastName;
    public string NickName { get; set; } = request.NickName;
}

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

            stringBuilder.AppendLine($"Validation error occurred at create user. Errors: ");
            result.Errors.ForEach(x => stringBuilder.AppendLine($"{x.Description} "));

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
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.NickName).NotEmpty();
    }
}