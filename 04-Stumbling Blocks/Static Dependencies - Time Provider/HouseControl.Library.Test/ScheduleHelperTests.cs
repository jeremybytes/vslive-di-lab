using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HouseControl.Library.Test
{
    [TestClass]
    public class ScheduleHelperTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Use current time provider as the default;
            // override in individual tests with "SetCurrentTime" if needed.
            ScheduleHelper.TimeProvider = new CurrentTimeProvider();
        }

        private static void SetCurrentTime(DateTime currentTime)
        {
            var mockTime = new Mock<ITimeProvider>();
            mockTime.Setup(t => t.Now()).Returns(currentTime);
            ScheduleHelper.TimeProvider = mockTime.Object;
        }

        [TestMethod]
        public void MondayItemInFuture_OnRollDay_IsUnchanged()
        {
            // Arrange
            DateTime monday = new DateTime(2020, 01, 06, 15, 32, 00);
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);
            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };

            // Act
            var newDate = ScheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(monday, newDate);
        }

        [TestMethod]
        public void MondayItemInFuture_OnRollWeekdayDay_IsUnchanged()
        {
            // Arrange
            DateTime monday = new DateTime(2020, 01, 06, 15, 32, 00);
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };

            // Act
            var newDate = ScheduleHelper.RollForwardToNextWeekdayDay(info);

            // Assert
            Assert.AreEqual(monday, newDate);
        }

        [TestMethod]
        public void SaturdayItemInFuture_OnRollDay_IsUnchanged()
        {
            // Arrange
            DateTime saturday = new DateTime(2020, 01, 11, 15, 32, 00);
            Assert.AreEqual(DayOfWeek.Saturday, saturday.DayOfWeek);
            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = saturday,
                TimeType = ScheduleTimeType.Standard,
            };

            // Act
            var newDate = ScheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(saturday, newDate);
        }

        [TestMethod]
        public void SaturdayItemInFuture_OnRollWeekendDay_IsUnchanged()
        {
            // Arrange
            DateTime saturday = new DateTime(2020, 01, 11, 15, 32, 00);
            Assert.AreEqual(DayOfWeek.Saturday, saturday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = saturday,
                TimeType = ScheduleTimeType.Standard,
            };

            // Act
            var newDate = ScheduleHelper.RollForwardToNextWeekendDay(info);

            // Assert
            Assert.AreEqual(saturday, newDate);
        }

        [TestMethod]
        public void MondayItemInPast_OnRollDay_IsTomorrow()
        {
            // Arrange
            var currentTime = new DateTime(2015, 01, 12, 16, 35, 22);
            SetCurrentTime(currentTime);

            var monday = new DateTime(2015, 01, 12, 15, 32, 00);
            var expectedTime = new DateTime(2015, 01, 13, 15, 32, 00);
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };

            // Act
            var newDateTime = ScheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(expectedTime, newDateTime);
        }
    }
}
