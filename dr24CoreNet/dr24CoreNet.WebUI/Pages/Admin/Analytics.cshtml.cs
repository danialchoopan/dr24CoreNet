using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace dr24CoreNet.WebUI.Pages.Admin;

public class AnalyticsModel : BasePageModel
{
    private readonly IUnitOfWork _uow;

    public AnalyticsModel(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public PlatformFinances Finances { get; set; } = new();
    public List<Appointment> RecentAppointments { get; set; } = new();

    public async Task OnGetAsync()
    {
        base.HandleLang();
        var allFinances = await _uow.PlatformFinances.GetAllAsync();
        Finances = allFinances.FirstOrDefault() ?? new PlatformFinances();

        var allAppointments = await _uow.Appointments.GetAllAsync();
        RecentAppointments = allAppointments.OrderByDescending(a => a.ReservedAt).Take(5).ToList();
    }
}
