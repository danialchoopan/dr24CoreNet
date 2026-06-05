using System.Text.Json;
using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using dr24CoreNet.Infrastructure.Persistence;

namespace dr24CoreNet.Infrastructure.Audit;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogActionAsync(string userId, string role, string action, string entityName, string entityId, object? before, object? after, string ip)
    {
        var log = new MedicalAuditLog
        {
            UserId = userId,
            ActorRole = role,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            BeforeState = before != null ? JsonSerializer.Serialize(before) : "{}",
            AfterState = after != null ? JsonSerializer.Serialize(after) : "{}",
            ClientIp = ip,
            Geolocation = "Simulated: Tehran, IR", // Real-world would use an IP lookup service
            Timestamp = DateTime.UtcNow
        };

        _context.MedicalAuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
