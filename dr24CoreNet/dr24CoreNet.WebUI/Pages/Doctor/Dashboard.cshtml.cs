using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dr24CoreNet.Application.DTOs;
using System.Text.Json;
using System.Text;

namespace dr24CoreNet.WebUI.Pages.Doctor;

public class DashboardModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public DashboardModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [BindProperty]
    public CreateTimeSlotsRequest SlotRequest { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _clientFactory.CreateClient("API");
        var content = new StringContent(JsonSerializer.Serialize(SlotRequest), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/doctors/generate-slots", content);
        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "اسلات‌های زمانی با موفقیت تولید شدند.";
        }

        return RedirectToPage();
    }
}
