using System;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Schedule;
namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class FieldLogicTests
    {
        #region Compact Tests

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void Compact_ReturnsOriginalForNonCompactableEmptyData()
        {
            string emptyPart = string.Empty;
            Assert.AreEqual("", new MinuteLogic().Compact(emptyPart));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void Compact_ReturnsOriginalForNonCompactableNullData()
        {
            string nullPart = null;
            Assert.AreEqual(null, new HourLogic().Compact(nullPart));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void Compact_ReturnsOriginalForNonCompactableCommasAndDashData()
        {
            string complexPart = "1,2,5-10";
            Assert.AreEqual("1,2,5-10", new DayLogic().Compact(complexPart));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void Compact_ReturnsOriginalForNonCompactableAsterixWithOneData()
        {
            string complexPart = "*/1";
            Assert.AreEqual("*/1", new MonthLogic().Compact(complexPart));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactMinute_ReturnsCompactedEvenDivisibleResult()
        {
            string expanded = "0,5,10,15,20,25,30,35,40,45,50,55";
            Assert.AreEqual("*/5", new MinuteLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactMinute_ReturnsCompactedOddDivisibleResult()
        {
            string expanded = "0,29,58";
            Assert.AreEqual("*/29", new MinuteLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactHour_ReturnsCompactedEvenDivisibleResult()
        {
            string expanded = "0,8,16";
            Assert.AreEqual("*/8", new HourLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactHour_ReturnsCompactedOddDivisibleResult()
        {
            string expanded = "0,11,22";
            Assert.AreEqual("*/11", new HourLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactDay_ReturnsCompactedOddDayResult()
        {
            string expanded = "1,16,31";
            Assert.AreEqual("*/15", new DayLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactDay_ReturnsCompactedOddMonthResult()
        {
            string expanded = "1,8";
            Assert.AreEqual("*/7", new MonthLogic().Compact(expanded));
        }

        [Test]
        [Category("ScheduleUtility.Compact")]
        public void CompactDay_ReturnsCompactedOddDayOfWeekResult()
        {
            string expanded = "0,3,6";
            Assert.AreEqual("0,3,6", new MonthLogic().Compact(expanded));
        }

        #endregion
    }
}
