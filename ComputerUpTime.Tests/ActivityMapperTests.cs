using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ComputerUpTime.Tests;

[ExcludeFromCodeCoverage]
public class ActivityMapperTests
{
    private ActivityMapper CreateSut(
        IEnumerable<WorkDayActivity> activities,
        out WorkDayLoggerMock loggerMock)
    {
        var timeMock = Substitute.For<ISystemTime>();
        timeMock.Now().Returns(DateTime.Parse("12.5.2021 00:00:00"));
        loggerMock = new WorkDayLoggerMock();
        var sut = new ActivityMapper(activities, timeMock, loggerMock);
        return sut;
    }

    [Fact]
    public void NoActivities_NoWorkDays()
    {
        var sut = CreateSut(Enumerable.Empty<WorkDayActivity>(), out var loggerMock);

        sut.Run();
        loggerMock.Output.Should().BeEquivalentTo(
            "The system event log does not contain any data.");
    }

    [Fact]
    public void SingleActivity_OneWorkDays()
    {
        var input = new[] {new WorkDayActivity(DateTime.Parse("12.4.2021 7:45:12"))};
        var sut = CreateSut(input, out var loggerMock);

        sut.Run();
        loggerMock.Output.Should().BeEquivalentTo(
            "Searching newest entries...",
            "12.04.2021: 07:45 - 07:45               (07:45:12 - 07:45:12)");
    }

    [Fact]
    public void ActivityOlderThan60Days_ActivityIsIgnored()
    {
        var input = new[] {new WorkDayActivity(DateTime.Parse("11.5.2021 23:59:59") - ActivityMapper.ActivityTimeLimit)};
        var sut = CreateSut(input, out var loggerMock);

        sut.Run();
        loggerMock.Output.Should().BeEquivalentTo(
            "Searching newest entries...",
            "The system event log does not contain any data for the last 60 days.");
    }

    [Fact]
    public void MultipleActivitiesInOneDay_OneWorkDays()
    {
        // var loggerMock = new WorkDayLoggerMock();
        var input = new[]
        {
            new WorkDayActivity(DateTime.Parse("12.4.2021 7:48:12")),
            new WorkDayActivity(DateTime.Parse("12.4.2021 14:00:00")),
            new WorkDayActivity(DateTime.Parse("12.4.2021 11:00:00")),
            new WorkDayActivity(DateTime.Parse("12.4.2021 16:32:14"))
        };
        // var sut = new ActivityMapper(input, loggerMock);
        var sut = CreateSut(input, out var loggerMock);

        sut.Run();
        loggerMock.Output.Should().BeEquivalentTo(
            "Searching newest entries...",
            "12.04.2021: 07:50 - 16:30               (07:48:12 - 16:32:14)");
    }

    [Fact]
    public void MultipleActivitiesOverMultipleDays_OneWorkDays()
    {
        // var loggerMock = new WorkDayLoggerMock();
        var input = new[]
        {
            new WorkDayActivity(DateTime.Parse("12.4.2021 14:00:00")),
            new WorkDayActivity(DateTime.Parse("12.4.2021 7:48:12")),
            new WorkDayActivity(DateTime.Parse("14.4.2021 8:12:30")),
            new WorkDayActivity(DateTime.Parse("14.4.2021 16:32:31"))
        };
        // var sut = new ActivityMapper(input, loggerMock);
        var sut = CreateSut(input, out var loggerMock);

        sut.Run();
        loggerMock.Output.Should().BeEquivalentTo(
            "Searching newest entries...",
            "12.04.2021: 07:50 - 14:00               (07:48:12 - 14:00:00)",
            "14.04.2021: 08:10 - 16:35               (08:12:30 - 16:32:31)");
    }
}