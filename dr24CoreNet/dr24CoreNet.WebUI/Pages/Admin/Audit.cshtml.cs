using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.WebUI.Pages.Admin;

public class AuditModel : BasePageModel
{
    private readonly IUnitOfWork _uow;

    public AuditModel(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public List<MedicalAuditLog> Logs { get; set; } = new();

    public async Task OnGetAsync()
    {
        base.HandleLang();
        var allLogs = await _uow.AuditLogs.GetAllAsync();
        Logs = allLogs.OrderByDescending(l => l.Timestamp).Take(10).ToList();
    }
}
