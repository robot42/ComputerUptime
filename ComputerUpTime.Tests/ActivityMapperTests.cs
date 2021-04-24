using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputerUpTime.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ActivityMapperTests
    {
        [TestMethod]
        public void NoActivities_NoWorkDays()
        {
            var loggerMock = new WorkDayLoggerMock();
            var sut = new ActivityMapper(Enumerable.Empty<WorkDayActivity>(), loggerMock);

            sut.Run();
            loggerMock.Output.Should().BeEquivalentTo(
                "The system event log does not contain any data.");
        }

        [TestMethod]
        public void SingleActivity_OneWorkDays()
        {
            var loggerMock = new WorkDayLoggerMock();
            var input = new [] {new WorkDayActivity(DateTime.Parse("12.4.2021 7:45:12"))};
            var sut = new ActivityMapper(input, loggerMock);

            sut.Run();
            loggerMock.Output.Should().BeEquivalentTo(
                "Searching newest entries...",
                "12.04.2021: 07:45 - 07:45               (07:45:12 - 07:45:12)");
        }

        [TestMethod]
        public void ActivityOlderThan60Days_ActivityIsIgnored()
        {
            var loggerMock = new WorkDayLoggerMock();
            var input = new[] { new WorkDayActivity(DateTime.Now - ActivityMapper.ActivityTimeLimit) };
            var sut = new ActivityMapper(input, loggerMock);

            sut.Run();
            loggerMock.Output.Should().BeEquivalentTo(
                "Searching newest entries...",
                "The system event log does not contain any data for the last 60 days.");
        }

        [TestMethod]
        public void MultipleActivitiesInOneDay_OneWorkDays()
        {
            var loggerMock = new WorkDayLoggerMock();
            var input = new[]
            {
                new WorkDayActivity(DateTime.Parse("12.4.2021 7:48:12")),
                new WorkDayActivity(DateTime.Parse("12.4.2021 14:00:00")),
                new WorkDayActivity(DateTime.Parse("12.4.2021 11:00:00")),
                new WorkDayActivity(DateTime.Parse("12.4.2021 16:32:14"))
            };
            var sut = new ActivityMapper(input, loggerMock);

            sut.Run();
            loggerMock.Output.Should().BeEquivalentTo(
                "Searching newest entries...",
                "12.04.2021: 07:50 - 16:30               (07:48:12 - 16:32:14)");
        }

        [TestMethod]
        public void MultipleActivitiesOverMultipleDays_OneWorkDays()
        {
            var loggerMock = new WorkDayLoggerMock();
            var input = new[]
            {
                new WorkDayActivity(DateTime.Parse("12.4.2021 14:00:00")),
                new WorkDayActivity(DateTime.Parse("12.4.2021 7:48:12")),
                new WorkDayActivity(DateTime.Parse("14.4.2021 8:12:30")),
                new WorkDayActivity(DateTime.Parse("14.4.2021 16:32:31"))
            };
            var sut = new ActivityMapper(input, loggerMock);

            sut.Run();
            loggerMock.Output.Should().BeEquivalentTo(
                "Searching newest entries...",
                "12.04.2021: 07:50 - 14:00               (07:48:12 - 14:00:00)",
                "14.04.2021: 08:10 - 16:35               (08:12:30 - 16:32:31)");
        }
    }
}