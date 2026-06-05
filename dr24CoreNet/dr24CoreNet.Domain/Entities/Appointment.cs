namespace dr24CoreNet.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int TimeSlotId { get; set; }
    public TimeSlot? TimeSlot { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public DateTime ReservedAt { get; set; }
    public decimal Fee { get; set; }
    public decimal Commission { get; set; }
}
