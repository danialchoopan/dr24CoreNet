using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace dr24CoreNet.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public PrescriptionsController(IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Prescription prescription)
    {
        prescription.IssuedAt = DateTime.UtcNow;
        await _unitOfWork.Prescriptions.AddAsync(prescription);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogActionAsync(
            "System",
            "Doctor",
            "ISSUE_PRESCRIPTION",
            "Prescription",
            prescription.Id.ToString(),
            null,
            prescription,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        );

        return Ok(new { Message = "Prescription issued successfully" });
    }
}
