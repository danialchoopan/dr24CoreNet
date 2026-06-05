using dr24CoreNet.Application.DTOs;
using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
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

    public AppointmentsController(IUnitOfWork unitOfWork, CommissionContext commissionContext)
    {
        _unitOfWork = unitOfWork;
        _commissionContext = commissionContext;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
    {
        var slot = await _unitOfWork.TimeSlots.GetByIdAsync(request.TimeSlotId);
        if (slot == null) return NotFound("Slot not found");
        if (slot.IsReserved) return Conflict("Slot is already reserved");

        var doctor = await _unitOfWork.Doctors.GetByIdAsync(slot.DoctorId);
        if (doctor == null) return NotFound("Doctor not found");

        var fee = 100000m; // Example fee
        var commission = _commissionContext.GetCommission(doctor.Specialization!.Type, fee);

        slot.IsReserved = true;

        var appointment = new Appointment
        {
            TimeSlotId = slot.Id,
            PatientId = request.PatientId,
            ReservedAt = DateTime.UtcNow,
            Fee = fee,
            Commission = commission
        };

        try
        {
            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new { Message = "Appointment booked successfully", AppointmentId = appointment.Id });
        }
        catch (DbUpdateConcurrencyException)
        {
            // Race condition handled!
            return Conflict("This slot was just booked by another user. Please try another slot.");
        }
    }
}
