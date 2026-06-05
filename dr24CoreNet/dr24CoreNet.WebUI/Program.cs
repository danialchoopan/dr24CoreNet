using dr24CoreNet.Infrastructure.Caching;
using dr24CoreNet.Infrastructure.Persistence;
using dr24CoreNet.Infrastructure.Repositories;
using dr24CoreNet.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
});

// Infrastructure registrations needed for UI if it uses repositories directly (e.g. for some logic)
// But following Clean Architecture, UI should primarily talk to API or have its own services.
// For this enterprise project, we'll keep it focused on API calls.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
