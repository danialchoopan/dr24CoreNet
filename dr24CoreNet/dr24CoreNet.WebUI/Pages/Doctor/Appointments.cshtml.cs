using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.WebUI.Pages.Doctor;

public class AppointmentsModel : BasePageModel
{
    private readonly IUnitOfWork _uow;

    public AppointmentsModel(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public List<Appointment> TodaysAppointments { get; set; } = new();
    public decimal TodaysRevenue { get; set; }

    public async Task OnGetAsync()
    {
        base.HandleLang();

        // Mocking "Today" as the date of some seeded appointments for the demo
        var allAppointments = await _uow.Appointments.GetAllAsync();
        TodaysAppointments = allAppointments.Take(3).ToList();
        TodaysRevenue = TodaysAppointments.Sum(a => a.Fee);
    }
}
