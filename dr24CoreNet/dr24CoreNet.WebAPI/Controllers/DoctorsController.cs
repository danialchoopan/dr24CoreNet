using dr24CoreNet.Application.DTOs;
using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Application.Services;
using dr24CoreNet.Infrastructure.Caching;
using Microsoft.AspNetCore.Mvc;

namespace dr24CoreNet.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CacheService _cacheService;
    private readonly SlotGeneratorService _slotGenerator;

    public DoctorsController(IUnitOfWork unitOfWork, CacheService cacheService, SlotGeneratorService slotGenerator)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _slotGenerator = slotGenerator;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? city, [FromQuery] string? specialization)
    {
        var cacheKey = $"Search_{city}_{specialization}";
        var doctors = await Task.FromResult(_cacheService.GetOrSet(cacheKey, () =>
            _unitOfWork.Doctors.SearchAsync(city, specialization).Result,
            TimeSpan.FromMinutes(10)));

        return Ok(doctors?.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            City = d.City,
            MedicalCouncilCode = d.MedicalCouncilCode,
            SpecializationName = d.Specialization?.Name ?? "General"
        }));
    }

    [HttpGet("{id}/slots")]
    public async Task<IActionResult> GetSlots(int id, [FromQuery] bool availableOnly = true)
    {
        var slots = await _unitOfWork.TimeSlots.GetByDoctorIdAsync(id, availableOnly);
        return Ok(slots.Select(s => new TimeSlotDto
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            IsReserved = s.IsReserved
        }));
    }

    [HttpPost("generate-slots")]
    public async Task<IActionResult> GenerateSlots([FromBody] CreateTimeSlotsRequest request)
    {
        var slots = _slotGenerator.GenerateSlots(request.DoctorId, request.Date, request.StartTime, request.EndTime, request.DurationMinutes);

        foreach (var slot in slots)
        {
            await _unitOfWork.TimeSlots.AddAsync(slot);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok(new { Message = $"{slots.Count} slots generated successfully." });
    }
}
