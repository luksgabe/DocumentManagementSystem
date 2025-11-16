using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DocumentManagement.Core.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _repo;
    private readonly ILogger<AuditService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(
        IAuditLogRepository repo,
        ILogger<AuditService> logger,
        IHttpContextAccessor accessor)
    {
        _repo = repo;
        _logger = logger;
        _httpContextAccessor = accessor;
    }

    public async Task LogAsync(string userSub, string userEmail, string action, string entity, string entityId, object? metadata = null)
    {
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var metadataJson = metadata != null
            ? JsonSerializer.Serialize(metadata)
            : null;

        var log = new AuditLog(
                   Guid.NewGuid(), 
                   DateTime.UtcNow, 
                   action,
                   userSub,
                   userEmail,
                   entity,
                   entityId,
                   ip ?? string.Empty,
                   metadataJson ?? string.Empty 
               );

        await _repo.AddAsync(log);

        _logger.LogInformation("Audit logged: {Action} {Entity}:{EntityId} by {User}",
            action, entity, entityId, userEmail);
    }
}