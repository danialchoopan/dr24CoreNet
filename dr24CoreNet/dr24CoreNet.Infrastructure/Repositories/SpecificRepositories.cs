using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using dr24CoreNet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace dr24CoreNet.Infrastructure.Repositories;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Doctor>> SearchAsync(string? city, string? specialization)
    {
        var query = _context.Doctors.Include(d => d.Specialization).AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(d => d.City.Contains(city));

        if (!string.IsNullOrEmpty(specialization))
            query = query.Where(d => d.Specialization!.Name.Contains(specialization));

        return await query.ToListAsync();
    }
}

public class TimeSlotRepository : Repository<TimeSlot>, ITimeSlotRepository
{
    public TimeSlotRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TimeSlot>> GetByDoctorIdAsync(int doctorId, bool onlyAvailable = false)
    {
        var query = _context.TimeSlots.Where(t => t.DoctorId == doctorId);

        if (onlyAvailable)
            query = query.Where(t => !t.IsReserved);

        return await query.OrderBy(t => t.StartTime).ToListAsync();
    }
}

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context) { }
}
