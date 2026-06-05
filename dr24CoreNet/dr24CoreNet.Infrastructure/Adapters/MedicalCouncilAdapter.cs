namespace dr24CoreNet.Infrastructure.Adapters;

// Legacy SOAP/WCF Service simulation
public class LegacyMedicalCouncilService
{
    public string CheckDoctorRegistration(int code, string secretKey)
    {
        // Simulated legacy response format
        return code > 1000 ? "VALID:REG-OK" : "INVALID:NOT-FOUND";
    }
}

public class MedicalCouncilAdapter : dr24CoreNet.Application.Interfaces.IMedicalCouncilAdapter
{
    private readonly LegacyMedicalCouncilService _legacyService;

    public MedicalCouncilAdapter()
    {
        _legacyService = new LegacyMedicalCouncilService();
    }

    public async Task<bool> ValidateCodeAsync(string code)
    {
        if (!int.TryParse(code, out int numericCode))
            return false;

        // Adapting the legacy call to modern interface
        var legacyResult = await Task.Run(() => _legacyService.CheckDoctorRegistration(numericCode, "INTERNAL_KEY"));

        return legacyResult.StartsWith("VALID");
    }
}
