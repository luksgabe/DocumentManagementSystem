using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult OkResult(object? data)
        => Ok(new { success = true, data });

    protected IActionResult ErrorResult(string message, int statusCode)
        => StatusCode(statusCode, new { success = false, error = message });

    protected IActionResult UnauthorizedResult(string message = "Invalid credentials")
        => Unauthorized(new { success = false, error = message });
}
