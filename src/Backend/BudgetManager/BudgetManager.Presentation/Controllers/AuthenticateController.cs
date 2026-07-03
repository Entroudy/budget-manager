namespace BudgetManager.Controllers;

[ApiController]
[Route("authenticate")]
public class AuthenticateController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthenticateController(IMediator mediator, ILogger<AuthenticateController> logger)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    [SwaggerOperation("Authenticates the user")]
    [SwaggerResponse(200, "Success", typeof(SuccessAuthenticateResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return Ok(await _mediator.Send(new UserAuthenticateCommand(request.Email, request.Password, request.TokenExpiration)));
    }
}

