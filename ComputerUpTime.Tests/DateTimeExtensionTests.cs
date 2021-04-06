using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputerUpTime.Tests
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        public void RoundToFiveMinutes_CloserToPreviousTime_RoundsDown()
        {
            var sut = new DateTime(2021, 4, 5, 14, 6, 12);
            var expected = new DateTime(2021, 4, 5, 14, 5, 0);

            sut.RoundToFiveMinutes().Should().Be(expected);
        }

        [TestMethod]
        public void RoundToFiveMinutes_CloserToNextTime_RoundsUp()
        {
            var sut = new DateTime(2021, 4, 5, 14, 9, 12);
            var expected = new DateTime(2021, 4, 5, 14, 10, 0);

            sut.RoundToFiveMinutes().Should().Be(expected);
        }

        [TestMethod]
        public void RoundToFiveMinutes_InTheMiddle_RoundsUp()
        {
            var sut = new DateTime(2021, 4, 5, 14, 7, 30);
            var expected = new DateTime(2021, 4, 5, 14, 10, 0);

            sut.RoundToFiveMinutes().Should().Be(expected);
        }

        [TestMethod]
        public void RoundToFiveMinutes_CorrectValueAlready_DoesNotRound()
        {
            var sut = new DateTime(2021, 4, 5, 14, 10, 00);
            var expected = new DateTime(2021, 4, 5, 14, 10, 0);

            sut.RoundToFiveMinutes().Should().Be(expected);
        }
    }
}