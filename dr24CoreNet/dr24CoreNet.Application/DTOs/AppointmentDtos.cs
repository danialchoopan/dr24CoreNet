namespace dr24CoreNet.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MedicalCouncilCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string SpecializationName { get; set; } = string.Empty;
}

public class TimeSlotDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsReserved { get; set; }
}

public class BookAppointmentRequest
{
    public int TimeSlotId { get; set; }
    public int PatientId { get; set; }
}

public class CreateTimeSlotsRequest
{
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationMinutes { get; set; }
}
