using dr24CoreNet.Domain.Entities;

namespace dr24CoreNet.Application.Services;

public class SlotGeneratorService
{
    public List<TimeSlot> GenerateSlots(int doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime, int durationMinutes)
    {
        var slots = new List<TimeSlot>();
        var currentStart = date.Date.Add(startTime);
        var endDateTime = date.Date.Add(endTime);

        while (currentStart.AddMinutes(durationMinutes) <= endDateTime)
        {
            slots.Add(new TimeSlot
            {
                DoctorId = doctorId,
                StartTime = currentStart,
                EndTime = currentStart.AddMinutes(durationMinutes),
                IsReserved = false
            });
            currentStart = currentStart.AddMinutes(durationMinutes);
        }

        return slots;
    }
}
