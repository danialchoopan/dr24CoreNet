using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Infrastructure.Persistence;
using dr24CoreNet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace dr24CoreNet.WebUI;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureForUI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Dr24Db"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
