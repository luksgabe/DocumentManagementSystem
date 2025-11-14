using DocumentManagement.Application.Auth.Commands;
using DocumentManagement.Application.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
            return UnauthorizedResult("Invalid email or password");

        return OkResult(result);
    }
}
