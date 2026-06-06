using dr24CoreNet.Domain.Entities;
using dr24CoreNet.Domain.Enums;
using dr24CoreNet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dr24CoreNet.Infrastructure.Seed;

public static class DbInitializer
{
    public static async Task Initialize(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (context.Doctors.Any()) return;

        // 1. Production-Scale Specializations
        var specs = new List<Specialization>
        {
            new Specialization { Name = "General Practitioner", Type = SpecializationType.General },
            new Specialization { Name = "Cardiologist", Type = SpecializationType.Specialist },
            new Specialization { Name = "Neurologist", Type = SpecializationType.SuperSpecialist },
            new Specialization { Name = "Oncologist", Type = SpecializationType.SuperSpecialist },
            new Specialization { Name = "Pediatrician", Type = SpecializationType.Specialist },
            new Specialization { Name = "Psychiatrist", Type = SpecializationType.SuperSpecialist },
            new Specialization { Name = "Orthopedic Surgeon", Type = SpecializationType.Specialist }
        };
        context.Specializations.AddRange(specs);
        await context.SaveChangesAsync();

        // 2. 50+ Realistic Doctors
        var doctors = new List<Doctor>();
        var firstNames = new[] { "Ali", "Reza", "Mohammad", "Sara", "Maryam", "Zahra", "Hassan", "Hossein", "Narges", "Omid" };
        var lastNames = new[] { "Ahmadi", "Rezayi", "Karimi", "Tehrani", "Hashemi", "Sadeghi", "Mohebbi", "Alavi", "Hosseini", "Abbasi" };
        var cities = new[] { "Tehran", "Isfahan", "Mashhad", "Shiraz", "Tabriz", "Karaj", "Ahvaz", "Qom" };

        for (int i = 0; i < 50; i++)
        {
            doctors.Add(new Doctor
            {
                Name = $"Dr. {firstNames[i % 10]} {lastNames[(i / 5) % 10]}",
                City = cities[i % 8],
                MedicalCouncilCode = (20000 + i).ToString(),
                SpecializationId = specs[i % specs.Count].Id
            });
        }
        context.Doctors.AddRange(doctors);
        await context.SaveChangesAsync();

        // 3. Thousands of Time Slots
        var slots = new List<TimeSlot>();
        var startDate = DateTime.UtcNow.Date.AddMonths(-1);
        for (int d = 0; d < 60; d++) // 60 days of data
        {
            var currentDay = startDate.AddDays(d);
            foreach (var doc in doctors.Take(20)) // Use subset for bulk slots
            {
                var startHour = currentDay.AddHours(9);
                for (int h = 0; h < 10; h++) // 10 slots per day
                {
                    slots.Add(new TimeSlot
                    {
                        DoctorId = doc.Id,
                        StartTime = startHour.AddMinutes(h * 30),
                        EndTime = startHour.AddMinutes((h + 1) * 30),
                        IsReserved = d < 30 // Old slots are reserved
                    });
                }
            }
        }
        context.TimeSlots.AddRange(slots);
        await context.SaveChangesAsync();

        // 4. Historical Appointments and Wallet Transactions
        var patients = new List<Patient>();
        for (int i = 1; i <= 30; i++)
        {
            var p = new Patient { Name = $"Patient_{i}", PhoneNumber = $"0912{i:D7}" };
            patients.Add(p);
            context.Patients.Add(p);
        }
        await context.SaveChangesAsync();

        foreach (var p in patients)
        {
            context.Wallets.Add(new Wallet { PatientId = p.Id, Balance = 10000000m });
        }
        await context.SaveChangesAsync();

        var historicalSlots = slots.Where(s => s.IsReserved).ToList();
        for (int i = 0; i < historicalSlots.Count; i++)
        {
            var s = historicalSlots[i];
            var doc = doctors.First(d => d.Id == s.DoctorId);
            decimal fee = 300000m;
            decimal comm = fee * 0.15m;

            context.Appointments.Add(new Appointment
            {
                TimeSlotId = s.Id,
                PatientId = patients[i % patients.Count].Id,
                ReservedAt = s.StartTime.AddHours(-24),
                Fee = fee,
                Commission = comm
            });
        }
        await context.SaveChangesAsync();

        // 5. Initial Analytics Snapshot
        var revenue = context.Appointments.Sum(a => a.Commission);
        var snapshot = new AnalyticsSnapshot
        {
            MetricName = "TotalRevenue",
            MetricValue = revenue.ToString(),
            LastUpdated = DateTime.UtcNow
        };
        context.AnalyticsSnapshots.Add(snapshot);

        // 6. Platform Finances and Doctor Accounts
        var platform = new PlatformFinances
        {
            TotalRevenue = context.Appointments.Sum(a => a.Fee),
            TotalCommission = context.Appointments.Sum(a => a.Commission),
            TotalPayouts = context.Appointments.Sum(a => a.Fee - a.Commission),
            LastUpdated = DateTime.UtcNow
        };
        context.PlatformFinances.Add(platform);

        foreach (var doc in doctors)
        {
            var docRevenue = context.Appointments
                .Where(a => a.TimeSlot!.DoctorId == doc.Id)
                .Sum(a => a.Fee - a.Commission);

            context.DoctorAccounts.Add(new DoctorAccount
            {
                DoctorId = doc.Id,
                PendingBalance = 0,
                WithdrawableBalance = docRevenue * 0.8m,
                TotalEarned = docRevenue
            });
        }

        await context.SaveChangesAsync();
    }
}
