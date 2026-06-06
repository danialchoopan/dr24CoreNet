using dr24CoreNet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainDoctor = dr24CoreNet.Domain.Entities.Doctor;
using DomainSpecialization = dr24CoreNet.Domain.Entities.Specialization;

namespace dr24CoreNet.WebUI.Pages;

public class IndexModel : BasePageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<DomainDoctor> FeaturedDoctors { get; set; } = new();

    public async Task OnGetAsync()
    {
        base.OnGet();
        try
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            FeaturedDoctors = doctors?.Take(2).ToList() ?? new List<DomainDoctor>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database access failed: {ex.Message}. Falling back to sample data for display.");
        }

        if (FeaturedDoctors == null || FeaturedDoctors.Count == 0)
        {
            if (Lang == "fa")
            {
                FeaturedDoctors = new List<DomainDoctor>
                {
                    new DomainDoctor { Name = "دکتر علی احمدی", City = "تهران", MedicalCouncilCode = "12345", Specialization = new DomainSpecialization { Name = "فوق تخصص قلب" } },
                    new DomainDoctor { Name = "دکتر سارا رضایی", City = "اصفهان", MedicalCouncilCode = "54321", Specialization = new DomainSpecialization { Name = "متخصص پوست" } }
                };
            }
            else
            {
                FeaturedDoctors = new List<DomainDoctor>
                {
                    new DomainDoctor { Name = "Dr. Ali Ahmadi", City = "Tehran", MedicalCouncilCode = "12345", Specialization = new DomainSpecialization { Name = "Cardiologist" } },
                    new DomainDoctor { Name = "Dr. Sara Rezayi", City = "Isfahan", MedicalCouncilCode = "54321", Specialization = new DomainSpecialization { Name = "Dermatologist" } }
                };
            }
        }
    }
}
