using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dr24CoreNet.Infrastructure.Persistence;

namespace dr24CoreNet.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("revenue-snapshot")]
    public async Task<IActionResult> GetRevenueSnapshot()
    {
        // Enterprise Level: Fetch from Materialized Snapshot Cache
        var snapshot = await _context.AnalyticsSnapshots
            .FirstOrDefaultAsync(s => s.MetricName == "RevenuePerSpecialty");

        if (snapshot == null) return NotFound("Snapshot not yet generated");
        return Ok(snapshot);
    }

    [HttpGet("commission-stats")]
    public async Task<IActionResult> GetCommissionStats()
    {
        var stats = await _context.Appointments
            .GroupBy(a => a.ReservedAt.Date)
            .Select(g => new {
                Date = g.Key,
                TotalCommission = g.Sum(x => x.Commission),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return Ok(stats);
    }

    [HttpGet("specialization-dist")]
    public async Task<IActionResult> GetSpecDistribution()
    {
        var dist = await _context.Appointments
            .Include(a => a.TimeSlot)
                .ThenInclude(t => t!.Doctor)
                    .ThenInclude(d => d!.Specialization)
            .GroupBy(a => a.TimeSlot!.Doctor!.Specialization!.Name)
            .Select(g => new { Specialization = g.Key, Count = g.Count() })
            .ToListAsync();

        return Ok(dist);
    }
}
