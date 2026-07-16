namespace BudgetManager.Controllers;

[ApiController]
[Route("applicationUsers")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;
    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpGet("")]
    [SwaggerOperation("Get user list")]
    [SwaggerResponse(200, "Success", typeof(PaginatedResult<UserResponse>))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserList([FromQuery] PaginationPostModel paginationPostModel)
    {
        return Ok(await _mediator.Send(new GetUserListQuery(paginationPostModel)));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpGet("roles")]
    [SwaggerOperation("Get assignable roles")]
    [SwaggerResponse(200, "Success", typeof(List<string>))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _mediator.Send(new GetRolesQuery()));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpGet("{applicationUserId}")]
    [SwaggerOperation("Get user by id")]
    [SwaggerResponse(200, "Success", typeof(UserResponse))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserById(Guid applicationUserId)
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery(applicationUserId)));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpPost("")]
    [SwaggerOperation("Creates a new user")]
    [SwaggerResponse(200, "Success", typeof(UserResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        return Ok(await _mediator.Send(new CreateUserCommand(request)));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpDelete("{applicationUserId}")]
    [SwaggerOperation("Deletes a user")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUser(string applicationUserId)
    {
        return Ok(await _mediator.Send(new DeleteUserCommand(applicationUserId)));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpPut("{applicationUserId}")]
    [SwaggerOperation("update the user")]
    [SwaggerResponse(200, "Success", typeof(UserResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateUser(string applicationUserId, [FromBody] UpdateUserRequest request)
    {
        return Ok(await _mediator.Send(new UpdateUserCommand(applicationUserId, request)));
    }

    [JwtAuthorize(AppRoles.Admin)]
    [HttpPut("{applicationUserId}/change-password")]
    [SwaggerOperation("Change the user password")]
    [SwaggerResponse(200, "Success", typeof(UserResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword(string applicationUserId, [FromBody] ChangePasswordRequest request)
    {
        return Ok(await _mediator.Send(new ChangePasswordCommand(applicationUserId, request)));
    }
}

