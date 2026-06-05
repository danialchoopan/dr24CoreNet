namespace dr24CoreNet.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<Appointment> Appointments { get; set; } = new();
}
