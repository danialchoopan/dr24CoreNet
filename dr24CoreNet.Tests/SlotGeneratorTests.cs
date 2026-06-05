using dr24CoreNet.Application.Services;
using dr24CoreNet.Domain.Entities;
using Xunit;

namespace dr24CoreNet.Tests;

public class SlotGeneratorTests
{
    [Fact]
    public void GenerateSlots_ShouldReturnCorrectNumberOfSlots()
    {
        // Arrange
        var service = new SlotGeneratorService();
        var date = DateTime.Today;
        var startTime = new TimeSpan(9, 0, 0);
        var endTime = new TimeSpan(10, 0, 0);
        var duration = 15;

        // Act
        var slots = service.GenerateSlots(1, date, startTime, endTime, duration);

        // Assert
        Assert.Equal(4, slots.Count);
        Assert.Equal(date.Add(startTime), slots[0].StartTime);
        Assert.Equal(date.Add(startTime).AddMinutes(15), slots[0].EndTime);
    }
}
