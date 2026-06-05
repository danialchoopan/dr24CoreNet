using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using dr24CoreNet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.Infrastructure.BackgroundWorkers;

public class AnalyticsSnapshotWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AnalyticsSnapshotWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Calculate Revenue per specialty
                var revenueBySpec = await context.Appointments
                    .Include(a => a.TimeSlot).ThenInclude(t => t!.Doctor).ThenInclude(d => d!.Specialization)
                    .GroupBy(a => a.TimeSlot!.Doctor!.Specialization!.Name)
                    .Select(g => new { Specialty = g.Key, Total = g.Sum(x => x.Commission) })
                    .ToListAsync();

                var snapshot = await context.AnalyticsSnapshots.FirstOrDefaultAsync(s => s.MetricName == "RevenuePerSpecialty")
                               ?? new AnalyticsSnapshot { MetricName = "RevenuePerSpecialty" };

                snapshot.MetricValue = JsonSerializer.Serialize(revenueBySpec);
                snapshot.LastUpdated = DateTime.UtcNow;

                if (snapshot.Id == 0) context.AnalyticsSnapshots.Add(snapshot);
                await context.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
