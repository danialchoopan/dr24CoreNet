namespace dr24CoreNet.Application.Interfaces;

public interface IAuditService
{
    Task LogActionAsync(string userId, string role, string action, string entityName, string entityId, object? before, object? after, string ip);
}
