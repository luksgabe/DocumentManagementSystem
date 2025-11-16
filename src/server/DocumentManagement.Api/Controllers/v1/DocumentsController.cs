using DocumentManagement.Application.Documents.Commands;
using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Application.Documents.Queries;
using DocumentManagement.Application.DocumentShares.Commands;
using DocumentManagement.Application.DocumentShares.DTOs;
using DocumentManagement.Application.DocumentShares.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocumentManagement.Api.Controllers.v1;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public sealed class DocumentsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocumentsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }


    [HttpGet]
    public async Task<IActionResult> GetDocuments(
        [FromQuery] string? search,
        [FromQuery] string? tag,
        [FromQuery] string? contentType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetDocumentsQuery(search, tag, contentType, page, pageSize));
        return OkResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDocumentByIdQuery(id));
        return OkResult(result);
    }

    [HttpPost]
    [RequestSizeLimit(15 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateDocumentDto dto)
    {
        var (sub, email) = GetCurrentUser();

        var command = new CreateDocumentCommand(
            dto.Title,
            dto.Description,
            (AccessType)dto.AccessType,
            dto.File,
            dto.Tags,
            sub,
            email);

        var result = await _mediator.Send(command);

        return OkResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDocumentCommand request)
    {
        if (id != request.Id)
            return ErrorResult("Route id and body id must match.", 400);

        var result = await _mediator.Send(request);
        return OkResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDocumentCommand(id));
        return OkResult(new { deleted = true });
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var dto = await _mediator.Send(new DownloadDocumentQuery(id));
        return File(dto.Content, dto.ContentType, dto.FileName);
    }

    [HttpPost("{id:guid}/share")]
    public async Task<IActionResult> ShareDocument(
    Guid id,
    [FromBody] ShareDocumentRequestDto body)
    {
        var (sub, email) = GetCurrentUser();
        var role = User.FindFirst("role")?.Value ?? string.Empty;

        var command = new ShareDocumentCommand(
            id,
            body.TargetType,
            body.TargetValue,
            body.Permission,
            sub,
            email,
            role
        );

        var result = await _mediator.Send(command);
        return OkResult(result);
    }

    [HttpGet("{id:guid}/share")]
    public async Task<IActionResult> GetShares(Guid id)
    {
        var result = await _mediator.Send(new GetDocumentSharesQuery(id));
        return OkResult(result);
    }

    [HttpDelete("{id:guid}/share/{shareId:guid}")]
    public async Task<IActionResult> Unshare(Guid id, Guid shareId)
    {
        var (sub, email) = GetCurrentUser();
        var role = User.FindFirst("role")?.Value ?? string.Empty;

        var command = new UnshareDocumentCommand(
            id,
            shareId,
            sub,
            email,
            role
        );

        await _mediator.Send(command);

        return OkResult(new { deleted = true });
    }

    #region Private methods
    private (string Sub, string Email) GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext!.User;

        var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? user.FindFirst("sub")?.Value;         

        var email = user.FindFirst(ClaimTypes.Email)?.Value
                    ?? user.FindFirst(ClaimTypes.Email)?.Value
                    ?? user.FindFirst("email")?.Value;       
        return (sub, email);
    }
    #endregion
}
