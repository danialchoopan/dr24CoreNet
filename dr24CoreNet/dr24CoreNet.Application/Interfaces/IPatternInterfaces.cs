using dr24CoreNet.Domain.Enums;

namespace dr24CoreNet.Application.Interfaces;

public interface ICommissionStrategy
{
    SpecializationType SpecializationType { get; }
    decimal Calculate(decimal fee);
}

public interface IMedicalCouncilAdapter
{
    Task<bool> ValidateCodeAsync(string code);
}
