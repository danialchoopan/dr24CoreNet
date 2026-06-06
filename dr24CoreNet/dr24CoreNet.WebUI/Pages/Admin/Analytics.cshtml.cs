using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dr24CoreNet.WebUI.Pages.Admin;

public class AnalyticsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public AnalyticsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public decimal TotalRevenue { get; set; }
    public decimal TotalCommission { get; set; }
    public int TodayAppointments { get; set; }
    public List<MedicalAuditLog> RecentAudits { get; set; } = new();
    public List<SpecialtyStats> SpecialtyDistribution { get; set; } = new();

    public async Task OnGetAsync()
    {
        var finances = (await _unitOfWork.PlatformFinances.GetAllAsync()).FirstOrDefault();
        if (finances != null)
        {
            TotalRevenue = finances.TotalRevenue;
            TotalCommission = finances.TotalCommission;
        }

        var appointments = await _unitOfWork.Appointments.GetAllAsync();
        TodayAppointments = appointments.Count(a => a.ReservedAt.Date == DateTime.Today);

        var audits = await _unitOfWork.AuditLogs.GetAllAsync();
        RecentAudits = audits.OrderByDescending(a => a.Timestamp).Take(5).ToList();

        var doctors = await _unitOfWork.Doctors.GetAllAsync();
        SpecialtyDistribution = doctors
            .GroupBy(d => d.Specialization?.Name ?? "نامشخص")
            .Select(g => new SpecialtyStats { Name = g.Key, Count = g.Count() })
            .OrderByDescending(s => s.Count)
            .ToList();
    }

    public class SpecialtyStats
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }
}
