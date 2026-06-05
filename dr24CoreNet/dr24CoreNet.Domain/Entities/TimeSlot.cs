namespace dr24CoreNet.Domain.Entities;

public class TimeSlot
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsReserved { get; set; }

    // Concurrency Token
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    // Enterprise Temporary Reservation
    public DateTime? ReservedUntil { get; set; }
    public int? TemporaryPatientId { get; set; }
}
