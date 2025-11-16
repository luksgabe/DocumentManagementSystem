using DocumentManagement.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Api.Controllers.v1;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class AuditController : ControllerBase
{
    private readonly IAuditLogRepository _repo;

    public AuditController(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var logs = await _repo.GetPagedAsync(page, pageSize);

        return Ok(new
        {
            success = true,
            data = logs
        });
    }
}
