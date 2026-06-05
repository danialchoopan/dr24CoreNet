using dr24CoreNet.Domain.Enums;

namespace dr24CoreNet.Domain.Entities;

public class Specialization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SpecializationType Type { get; set; }
    public List<Doctor> Doctors { get; set; } = new();
}
