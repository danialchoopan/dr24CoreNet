using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using dr24CoreNet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace dr24CoreNet.Infrastructure.BackgroundWorkers;

public class ReservationCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ReservationCleanupWorker(IServiceProvider serviceProvider)
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

                // Release expired temporary reservations
                var expiredSlots = await context.TimeSlots
                    .Where(s => !s.IsReserved && s.ReservedUntil != null && s.ReservedUntil < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var slot in expiredSlots)
                {
                    slot.ReservedUntil = null;
                    slot.TemporaryPatientId = null;
                }

                if (expiredSlots.Any())
                {
                    await context.SaveChangesAsync();
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
