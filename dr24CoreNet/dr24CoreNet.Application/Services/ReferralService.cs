using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.Application.Services;

public class ReferralService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReferralService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateReferralAsync(int sourceDoctorId, int targetDoctorId, int patientId)
    {
        var token = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        var referral = new DoctorReferral
        {
            ReferralToken = token,
            SourceDoctorId = sourceDoctorId,
            TargetDoctorId = targetDoctorId,
            PatientId = patientId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsUsed = false
        };

        // In a real system, we'd add DoctorReferrals to Unit of Work
        // For this phase, we assume the generic repository handles it
        await Task.CompletedTask; // Placeholder for context registration
        return token;
    }
}
