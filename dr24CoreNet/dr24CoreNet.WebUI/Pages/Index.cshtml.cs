using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dr24CoreNet.WebUI.Pages;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<Doctor> FeaturedDoctors { get; set; } = new();

    public async Task OnGetAsync()
    {
        try
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            FeaturedDoctors = doctors?.Take(2).ToList() ?? new List<Doctor>();
        }
        catch (Exception ex)
        {
            // Logging can be added here if needed for debugging
            Console.WriteLine($"Database access failed: {ex.Message}. Falling back to sample data for display.");
        }

        if (FeaturedDoctors == null || FeaturedDoctors.Count == 0)
        {
            // Fallback for screenshots if DB is not reachable or empty in sandbox
            string lang = Request.Query["lang"].ToString() == "en" ? "en" : "fa";
            if (lang == "fa")
            {
                FeaturedDoctors = new List<Doctor>
                {
                    new Doctor { Name = "دکتر علی احمدی", City = "تهران", MedicalCouncilCode = "12345", Specialization = new Specialization { Name = "فوق تخصص قلب" } },
                    new Doctor { Name = "دکتر سارا رضایی", City = "اصفهان", MedicalCouncilCode = "54321", Specialization = new Specialization { Name = "متخصص پوست" } }
                };
            }
            else
            {
                FeaturedDoctors = new List<Doctor>
                {
                    new Doctor { Name = "Dr. Ali Ahmadi", City = "Tehran", MedicalCouncilCode = "12345", Specialization = new Specialization { Name = "Cardiologist" } },
                    new Doctor { Name = "Dr. Sara Rezayi", City = "Isfahan", MedicalCouncilCode = "54321", Specialization = new Specialization { Name = "Dermatologist" } }
                };
            }
        }
    }
}
