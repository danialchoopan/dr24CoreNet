using dr24CoreNet.Application.DTOs;
using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using dr24CoreNet.Infrastructure.Concurrency;
using dr24CoreNet.Infrastructure.Strategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dr24CoreNet.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CommissionContext _commissionContext;
    private readonly DistributedLockService _lockService;
    private readonly IAuditService _auditService;

    public AppointmentsController(
        IUnitOfWork unitOfWork,
        CommissionContext commissionContext,
        DistributedLockService lockService,
        IAuditService auditService)
    {
        _unitOfWork = unitOfWork;
        _commissionContext = commissionContext;
        _lockService = lockService;
        _auditService = auditService;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
    {
        string lockKey = $"slot_{request.TimeSlotId}";

        // Step 1: Pessimistic Lock (Distributed Lock)
        if (!await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(5)))
        {
            return StatusCode(423, "System is busy processing this slot. Please try again in a few seconds.");
        }

        try
        {
            var slot = await _unitOfWork.TimeSlots.GetByIdAsync(request.TimeSlotId);
            if (slot == null) return NotFound("Slot not found");

            // Check if already reserved or temporarily reserved
            if (slot.IsReserved || (slot.ReservedUntil.HasValue && slot.ReservedUntil > DateTime.UtcNow))
            {
                return Conflict("Slot is already reserved or in process of reservation.");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(slot.DoctorId);
            if (doctor == null) return NotFound("Doctor not found");

            var fee = 100000m; // Example fee
            var commission = _commissionContext.GetCommission(doctor.Specialization!.Type, fee);

            // Set temporary reservation for 10 minutes
            slot.ReservedUntil = DateTime.UtcNow.AddMinutes(10);
            slot.IsReserved = true;

            var appointment = new Appointment
            {
                TimeSlotId = slot.Id,
                PatientId = request.PatientId,
                ReservedAt = DateTime.UtcNow,
                Fee = fee,
                Commission = commission
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            // Audit Log
            await _auditService.LogActionAsync(
                request.PatientId.ToString(),
                "Patient",
                "BOOK_APPOINTMENT",
                "Appointment",
                appointment.Id.ToString(),
                null,
                appointment,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            );

            return Ok(new { Message = "Appointment booked successfully", AppointmentId = appointment.Id });
        }
        catch (DbUpdateConcurrencyException)
        {
            // Step 2: Optimistic Concurrency Fallback
            return Conflict("This slot was just booked by another user. Please try another slot.");
        }
        finally
        {
            _lockService.ReleaseLock(lockKey);
        }
    }
}
