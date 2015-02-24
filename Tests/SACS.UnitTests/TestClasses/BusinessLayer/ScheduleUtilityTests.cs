using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACS.BusinessLayer.BusinessLogic.Schedule;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestClass]
    public class ScheduleUtilityTests
    {
        #region GetFullDescription Tests
        
        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_DoesNotBreakOnEmptyOrInvalid()
        {
            string desc = ScheduleUtility.GetFullDescription(string.Empty);
            Assert.IsTrue(true, "No crash");
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_ReturnsDescriptionForDefaultSchedule()
        {
            string desc = ScheduleUtility.GetFullDescription("* * * * *");
            Assert.AreEqual(ScheduleUtility.AnyTimeDescription, desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryFewMintues()
        {
            string desc = ScheduleUtility.GetFullDescription("*/3 * * * *");
            Assert.AreEqual("Every 3 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtASetMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("15 * * * *");
            Assert.AreEqual("Every hour, at 15 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSetMinutes()
        {
            string desc = ScheduleUtility.GetFullDescription("5,10,11-15,30 * * * *");
            Assert.AreEqual("Every hour, at 5,10-15,30 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryFewHoursEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* */4 * * *");
            Assert.AreEqual("Every 4 hours, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryFewHoursAtASetMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("15 */4 * * *");
            Assert.AreEqual("Every 4 hours, at 15 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtASetHourEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* 6 * * *");
            Assert.AreEqual("Every day, at 6am, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSetHoursEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* 0,8,12-16 * * *");
            Assert.AreEqual("Every day, at midnight,8am,12pm-4pm, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSetTimesEveryDay()
        {
            string desc = ScheduleUtility.GetFullDescription("0,20 0,12-14 * * *");
            Assert.AreEqual("Every day, at 00:00,00:20,12:00,12:20-14:00,14:20", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryFewDaysEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* * */3 * *");
            Assert.AreEqual("Every 3 days, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtASetDayEveryFewHoursEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* */3 18 * *");
            Assert.AreEqual("Every month, on the 18th day, every 3 hours, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSetDaysEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* * 1-3,11,15,22 * *");
            Assert.AreEqual("Every month, on the 1st-3rd,11th,15th,22nd days, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryFewMonthsEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* * * */2 *");
            Assert.AreEqual("Every 2 months, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtASetMonthEveryFewHoursEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("*/10 * * 5 *");
            Assert.AreEqual("Any day of the week, in May, every 10 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSetMonthsEveryFewHoursEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* */3 * 1,2,7-9,12 *");
            Assert.AreEqual("Any day of the week, in January-February,July-September,December, every 3 hours, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_EveryOtherDayOfTheWeekEveryMinute()
        {
            string desc = ScheduleUtility.GetFullDescription("* * * * */2");
            Assert.AreEqual("On Sunday,Tuesday,Thursday,Saturday, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AtSpecificDaysOfTheWeekEveryOddDay()
        {
            string desc = ScheduleUtility.GetFullDescription("* * */5 * 1-3,5,6");
            Assert.AreEqual("On Monday-Wednesday,Friday-Saturday, every 5 days, every minute", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_FullEveryTest()
        {
            string desc = ScheduleUtility.GetFullDescription("*/5 */5 */5 */5 */5");
            Assert.AreEqual("On Sunday,Friday, every 5 months, every 5 days, every 5 hours, every 5 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_AlternatingSetValues()
        {
            string desc = ScheduleUtility.GetFullDescription("4-6 * 4-6 * 4-6");
            Assert.AreEqual("On Thursday-Saturday, every month, on the 4th-6th day, every hour, at 4-6 minutes", desc);
        }

        [TestMethod]
        [TestCategory("ScheduleUtility.GetFullDescription")]
        public void GetFullDescription_ThatBizarrelySpecificDayOfTheYearThatWillRunOnceEverySevenYears()
        {
            string desc = ScheduleUtility.GetFullDescription("30 16 24 4 5");
            Assert.AreEqual("On Friday, in April, on the 24th day, at 16:30", desc);
        }

        #endregion
    }
}
