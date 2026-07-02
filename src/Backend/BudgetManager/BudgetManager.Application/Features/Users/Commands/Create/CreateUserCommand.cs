using BudgetManager.Application.Models;
using BudgetManager.Domain.Entities;
using MediatR;

namespace BudgetManager.Application;

public class CreateUserCommand(CreateUserRequest request) : IRequest<Unit>
{
    public string Email { get; set; } = request.Email;
    public string Password { get; set; } = request.Password;
    public string FirstName { get; set; } = request.FirstName;
    public string LastName { get; set; } = request.LastName;
    public string NickName { get; set; } = request.NickName;
}
