using CleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Shared.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected string? GetUserId()
    {
        return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
} 