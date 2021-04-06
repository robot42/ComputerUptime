using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputerUpTime.Tests
{
    [TestClass]
    public class WorkDayTests
    {
        [TestMethod]
        public void Constructor_InitialValuesAreCorrect()
        {
            var dateTime = new DateTime(2021, 4, 5, 14, 6, 12);
            var sut = new WorkDay(dateTime);

            sut.Start.Should().Be(dateTime);
            sut.End.Should().Be(dateTime);
        }
    }
}