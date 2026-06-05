using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using dr24CoreNet.Infrastructure.Persistence;

namespace dr24CoreNet.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IDoctorRepository Doctors { get; }
    public ITimeSlotRepository TimeSlots { get; }
    public IAppointmentRepository Appointments { get; }

    // Generic Repositories for advanced entities for brevity in this complex task
    public IRepository<Wallet> Wallets { get; }
    public IRepository<Prescription> Prescriptions { get; }
    public IRepository<PlatformFinances> PlatformFinances { get; }
    public IRepository<DoctorAccount> DoctorAccounts { get; }
    public IRepository<MedicalAuditLog> AuditLogs { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Doctors = new DoctorRepository(_context);
        TimeSlots = new TimeSlotRepository(_context);
        Appointments = new AppointmentRepository(_context);
        Wallets = new Repository<Wallet>(_context);
        Prescriptions = new Repository<Prescription>(_context);
        PlatformFinances = new Repository<PlatformFinances>(_context);
        DoctorAccounts = new Repository<DoctorAccount>(_context);
        AuditLogs = new Repository<MedicalAuditLog>(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
