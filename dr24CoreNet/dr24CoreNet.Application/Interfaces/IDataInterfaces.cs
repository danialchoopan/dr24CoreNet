using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<IEnumerable<Doctor>> SearchAsync(string? city, string? specialization);
}

public interface ITimeSlotRepository : IRepository<TimeSlot>
{
    Task<IEnumerable<TimeSlot>> GetByDoctorIdAsync(int doctorId, bool onlyAvailable = false);
}

public interface IAppointmentRepository : IRepository<Appointment>
{
}

public interface IUnitOfWork : IDisposable
{
    IDoctorRepository Doctors { get; }
    ITimeSlotRepository TimeSlots { get; }
    IAppointmentRepository Appointments { get; }
    IRepository<dr24CoreNet.Domain.Entities.Wallet> Wallets { get; }
    IRepository<dr24CoreNet.Domain.Entities.Prescription> Prescriptions { get; }
    Task<int> SaveChangesAsync();
}
