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

    public async Task<string> CreateReferralAsync(int sourceDoctorId, int targetSpecializationId, int patientId, string notes)
    {
        var token = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        var referral = new DoctorReferral
        {
            ReferralToken = token,
            SourceDoctorId = sourceDoctorId,
            TargetDoctorId = 0, // Assigned when patient picks a doctor
            PatientId = patientId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsUsed = false
        };

        // In a real system, we'd add DoctorReferrals to Unit of Work
        await Task.CompletedTask;
        return token;
    }

    public async Task<DoctorReferral?> ValidateReferralAsync(string token)
    {
        // Mock validation for enterprise flow
        await Task.CompletedTask;
        return new DoctorReferral { ReferralToken = token, IsUsed = false, ExpiresAt = DateTime.UtcNow.AddDays(1) };
    }
}
