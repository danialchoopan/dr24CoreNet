namespace dr24CoreNet.Domain.Entities;

public class MedicalAuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ActorRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string BeforeState { get; set; } = string.Empty; // JSON
    public string AfterState { get; set; } = string.Empty;  // JSON
    public string ClientIp { get; set; } = string.Empty;
    public string Geolocation { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class DoctorReferral
{
    public int Id { get; set; }
    public string ReferralToken { get; set; } = string.Empty;
    public int SourceDoctorId { get; set; }
    public Doctor? SourceDoctor { get; set; }
    public int TargetDoctorId { get; set; }
    public Doctor? TargetDoctor { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class AnalyticsSnapshot
{
    public int Id { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public string MetricValue { get; set; } = string.Empty; // JSON or stringified value
    public DateTime LastUpdated { get; set; }
}
