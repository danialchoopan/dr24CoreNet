namespace dr24CoreNet.Domain.Entities;

public class PlatformFinances
{
    public int Id { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCommission { get; set; }
    public decimal TotalPayouts { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class DoctorAccount
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public decimal PendingBalance { get; set; }
    public decimal WithdrawableBalance { get; set; }
    public decimal TotalEarned { get; set; }
}
