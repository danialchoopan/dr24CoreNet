using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
namespace dr24CoreNet.Application.Services;

public class WalletService
{
    private readonly IUnitOfWork _unitOfWork;

    public WalletService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RefundAppointmentAsync(int appointmentId)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
        if (appointment == null) return;

        // Atomic transaction using Unit of Work
        var wallet = await _unitOfWork.Wallets.GetAllAsync(); // Simplified lookup
        var patientWallet = wallet.FirstOrDefault(w => w.PatientId == appointment.PatientId);

        if (patientWallet != null)
        {
            patientWallet.Balance += appointment.Fee;
            patientWallet.Transactions.Add(new WalletTransaction
            {
                Amount = appointment.Fee,
                TransactionDate = DateTime.UtcNow,
                Description = $"Refund for appointment {appointmentId} (Doctor Cancelled)",
                IsCredit = true
            });

            // Mark slot as free again
            var slot = await _unitOfWork.TimeSlots.GetByIdAsync(appointment.TimeSlotId);
            if (slot != null) slot.IsReserved = false;

            _unitOfWork.Appointments.Delete(appointment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
