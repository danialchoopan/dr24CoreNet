using dr24CoreNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dr24CoreNet.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    // Advanced & Enterprise Entities
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();

    public DbSet<MedicalAuditLog> MedicalAuditLogs => Set<MedicalAuditLog>();
    public DbSet<DoctorReferral> DoctorReferrals => Set<DoctorReferral>();
    public DbSet<AnalyticsSnapshot> AnalyticsSnapshots => Set<AnalyticsSnapshot>();

    public DbSet<PlatformFinances> PlatformFinances => Set<PlatformFinances>();
    public DbSet<DoctorAccount> DoctorAccounts => Set<DoctorAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimeSlot>()
            .Property(t => t.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Specialization)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecializationId);

        modelBuilder.Entity<TimeSlot>()
            .HasOne(t => t.Doctor)
            .WithMany(d => d.TimeSlots)
            .HasForeignKey(t => t.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.TimeSlot)
            .WithOne()
            .HasForeignKey<Appointment>(a => a.TimeSlotId);

        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.Patient)
            .WithOne()
            .HasForeignKey<Wallet>(w => w.PatientId);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Appointment)
            .WithOne()
            .HasForeignKey<Prescription>(p => p.AppointmentId);

        base.OnModelCreating(modelBuilder);
    }
}
