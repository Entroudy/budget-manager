namespace BudgetManager.Controllers;

[ApiController]
[Route("incomes")]
public class IncomeController : ControllerBase
{
    private readonly ILogger<IncomeController> _logger;
    private readonly IMediator _mediator;

    public IncomeController(ILogger<IncomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [JwtAuthorize()]
    [HttpGet("user/{applicationUserId}")]
    [SwaggerOperation("Get incomes by user id")]
    [SwaggerResponse(200, "Success", typeof(List<IncomeResponse>))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetIncomesByUserId(Guid applicationUserId)
    {
        return Ok(await _mediator.Send(new GetIncomesByUserIdQuery(applicationUserId)));
    }

    [JwtAuthorize()]
    [HttpPost("")]
    [SwaggerOperation("Creates a new income")]
    [SwaggerResponse(200, "Success", typeof(IncomeResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest request)
    {
        return Ok(await _mediator.Send(new CreateIncomeCommand(request)));
    }

    [JwtAuthorize()]
    [HttpPut("{id}")]
    [SwaggerOperation("Updates an income")]
    [SwaggerResponse(200, "Success", typeof(IncomeResponse))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Not found", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeRequest request)
    {
        return Ok(await _mediator.Send(new UpdateIncomeCommand(id, request)));
    }

    [JwtAuthorize()]
    [HttpDelete("{id}")]
    [SwaggerOperation("Deletes an income")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Not found", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteIncomeCommand(id)));
    }
}
