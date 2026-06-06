using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.WebUI.Pages.Patient;

public class HistoryModel : BasePageModel
{
    private readonly IUnitOfWork _uow;

    public HistoryModel(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public List<Appointment> Appointments { get; set; } = new();

    public async Task OnGetAsync()
    {
        base.HandleLang();
        var allAppointments = await _uow.Appointments.GetAllAsync();
        // For the demo we just take all
        Appointments = allAppointments.OrderByDescending(a => a.ReservedAt).ToList();
    }
}
