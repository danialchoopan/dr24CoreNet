namespace dr24CoreNet.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MedicalCouncilCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }
    public List<TimeSlot> TimeSlots { get; set; } = new();
}
