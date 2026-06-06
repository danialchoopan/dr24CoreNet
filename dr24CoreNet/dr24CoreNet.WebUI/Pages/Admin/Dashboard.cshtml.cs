using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.WebUI.Pages.Admin;

public class DashboardModel : BasePageModel
{
    private readonly IUnitOfWork _uow;

    public DashboardModel(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public PlatformFinances Finances { get; set; } = new();
    public List<DoctorAccount> TopDoctorAccounts { get; set; } = new();

    public async Task OnGetAsync()
    {
        base.HandleLang();
        var allFinances = await _uow.PlatformFinances.GetAllAsync();
        Finances = allFinances.FirstOrDefault() ?? new PlatformFinances();

        var allAccounts = await _uow.DoctorAccounts.GetAllAsync();
        TopDoctorAccounts = allAccounts.OrderByDescending(a => a.TotalEarned).Take(3).ToList();
    }
}
