using BudgetManager.Application.Models;
using MediatR;

namespace BudgetManager.Application;

public class ChangePasswordCommand(string id, ChangePasswordRequest request) : IRequest<Unit>
{
    public string ApplicationUserId { get; set; } = id;
    public string Password { get; set; } = request.Password;
}
