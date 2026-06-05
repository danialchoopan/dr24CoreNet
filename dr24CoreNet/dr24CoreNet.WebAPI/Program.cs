using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Application.Services;
using dr24CoreNet.Infrastructure.Adapters;
using dr24CoreNet.Infrastructure.Caching;
using dr24CoreNet.Infrastructure.Persistence;
using dr24CoreNet.Infrastructure.Repositories;
using dr24CoreNet.Infrastructure.Seed;
using dr24CoreNet.Infrastructure.Strategies;
using Microsoft.EntityFrameworkCore;
using dr24CoreNet.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMedicalCouncilAdapter, MedicalCouncilAdapter>();
builder.Services.AddScoped<SlotGeneratorService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<ReferralService>();
builder.Services.AddScoped<IAuditService, dr24CoreNet.Infrastructure.Audit.AuditService>();
builder.Services.AddSingleton<dr24CoreNet.Infrastructure.Concurrency.DistributedLockService>();
builder.Services.AddHostedService<dr24CoreNet.Infrastructure.BackgroundWorkers.ReservationCleanupWorker>();
builder.Services.AddHostedService<dr24CoreNet.Infrastructure.BackgroundWorkers.AnalyticsSnapshotWorker>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheService>();

// Strategy Pattern
builder.Services.AddScoped<ICommissionStrategy, GeneralCommissionStrategy>();
builder.Services.AddScoped<ICommissionStrategy, SpecialistCommissionStrategy>();
builder.Services.AddScoped<ICommissionStrategy, SuperSpecialistCommissionStrategy>();
builder.Services.AddScoped<CommissionContext>();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("Database seeding skipped or failed (Postgres might not be ready): " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
