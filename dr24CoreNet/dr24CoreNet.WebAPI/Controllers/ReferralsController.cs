using dr24CoreNet.Application.Services;
using dr24CoreNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace dr24CoreNet.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferralsController : ControllerBase
{
    private readonly ReferralService _referralService;

    public ReferralsController(ReferralService referralService)
    {
        _referralService = referralService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateReferral(int fromDoctorId, int patientId, int toSpecializationId, string notes)
    {
        var referral = await _referralService.CreateReferralAsync(fromDoctorId, toSpecializationId, patientId, notes);
        return Ok(referral);
    }

    [HttpGet("validate/{code}")]
    public async Task<IActionResult> ValidateReferral(string code)
    {
        var referral = await _referralService.ValidateReferralAsync(code);
        if (referral == null) return NotFound("Invalid or expired referral code");
        return Ok(referral);
    }
}
