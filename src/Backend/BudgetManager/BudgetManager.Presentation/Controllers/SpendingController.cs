using BudgetManager.Application;
using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Requests;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BudgetManager.Controllers;

[ApiController]
[Route("spendings")]
public class SpendingController : ControllerBase
{
    private readonly ILogger<SpendingController> _logger;
    private readonly IMediator _mediator;

    public SpendingController(ILogger<SpendingController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [JwtAuthorize()]
    [HttpGet("")]
    [SwaggerOperation("Get spending list")]
    [SwaggerResponse(200, "Success", typeof(PaginatedResult<SpendingListResponse>))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetSpendingList([FromQuery] SpendingFilterPostModel filterPostModel)
    {
        return Ok(await _mediator.Send(new GetSpendingListQuery(filterPostModel)));
    }

    [JwtAuthorize()]
    [HttpGet("{id}")]
    [SwaggerOperation("Get spending by id")]
    [SwaggerResponse(200, "Success", typeof(SpendingResponse))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Not found", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> GetSpendingById(Guid id)
    {
        return Ok(await _mediator.Send(new GetSpendingByIdQuery(id)));
    }

    [JwtAuthorize()]
    [HttpPost("")]
    [SwaggerOperation("Creates a new spending")]
    [SwaggerResponse(200, "Success", typeof(int))]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateSpending([FromBody] CreateSpendingRequest request)
    {
        var id = await _mediator.Send(new CreateSpendingCommand(request));
        return Ok(id);
    }

    [JwtAuthorize()]
    [HttpPut("{id}")]
    [SwaggerOperation("Updates a spending")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Not found", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateSpending(Guid id, [FromBody] UpdateSpendingRequest request)
    {
        var command = new UpdateSpendingCommand(request)
        {
            Id = id
        };
        return Ok(await _mediator.Send(command));
    }

    [JwtAuthorize()]
    [HttpDelete("{id}")]
    [SwaggerOperation("Deletes a spending")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Bad request", typeof(ProblemDetails))]
    [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
    [SwaggerResponse(404, "Not found", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteSpending(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteSpendingCommand(id)));
    }
}
