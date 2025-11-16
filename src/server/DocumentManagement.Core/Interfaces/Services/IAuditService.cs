namespace DocumentManagement.Core.Interfaces.Services;

public interface IAuditService
{
    Task LogAsync(string userSub, string userEmail, string action, string entity, string entityId, object? metadata = null);
}
