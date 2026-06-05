using dr24CoreNet.Application.Interfaces;
using dr24CoreNet.Domain.Enums;

namespace dr24CoreNet.Infrastructure.Strategies;

public class GeneralCommissionStrategy : ICommissionStrategy
{
    public SpecializationType SpecializationType => SpecializationType.General;
    public decimal Calculate(decimal fee) => fee * 0.10m;
}

public class SpecialistCommissionStrategy : ICommissionStrategy
{
    public SpecializationType SpecializationType => SpecializationType.Specialist;
    public decimal Calculate(decimal fee) => fee * 0.15m;
}

public class SuperSpecialistCommissionStrategy : ICommissionStrategy
{
    public SpecializationType SpecializationType => SpecializationType.SuperSpecialist;
    public decimal Calculate(decimal fee) => fee * 0.20m;
}

public class CommissionContext
{
    private readonly IEnumerable<ICommissionStrategy> _strategies;

    public CommissionContext(IEnumerable<ICommissionStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal GetCommission(SpecializationType type, decimal fee)
    {
        var strategy = _strategies.FirstOrDefault(s => s.SpecializationType == type);
        return strategy?.Calculate(fee) ?? 0;
    }
}
