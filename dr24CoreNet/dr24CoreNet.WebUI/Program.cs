using dr24CoreNet.Infrastructure.Caching;
using dr24CoreNet.Infrastructure.Persistence;
using dr24CoreNet.Infrastructure.Repositories;
using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.WebUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
});

builder.Services.AddInfrastructureForUI(builder.Configuration);

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
