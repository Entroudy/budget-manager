namespace BudgetManager.Application;

public class UpdateUserCommand(string id, UpdateUserRequest request) : IRequest<Unit>
{
    public string ApplicationUserId { get; set; } = id;
    public string Email { get; set; } = request.Email;
    public string FirstName { get; set; } = request.FirstName;
    public string LastName { get; set; } = request.LastName;
    public string NickName { get; set; } = request.NickName;
    public string? Role { get; set; } = request.Role;
    public string? ProfileImage { get; set; } = request.ProfileImage;
}
