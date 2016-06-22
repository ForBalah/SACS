using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.DataAccessLayer
{
    [TestFixture]
    public class SystemPerformanceTests
    {
        private const int secondsInterval = 90;

        #region Compact Data

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithEmptyList()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>();
            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>(), data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompatData_WorksWithSingleEntry()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 }
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 }
            }, data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithTwoEntriesThatAreTheSameValue()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10 },
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10 },
            }, data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithTwoEntriesThatAreDifferent()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 20 },
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 20 },
            }, data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithTwoEntriesThatAreFarApart()
        {
            // Since it will be greater than the secondsInterval, 2 should be inserted to represent the gap
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 10, 0), Value = 10 },
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 1), Value = 0 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 9, 59), Value = 0 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 10, 0), Value = 10 },
            }, data.ToList(), string.Format("Different number of elements: {0}", data.Count));
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithThreeEntriesThatAreDifferent()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 11 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 11 },
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 11 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 11 },
            }, data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        public void CompactData_WorksWithThreeEntriesThatAreTheSame()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 10 },
            };

            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 10 },
            }, data.ToList());
        }

        #endregion Compact Data

        #region Lower Resolution

        [Category("SystemPerformanceTests")]
        [Test]
        public void LowerResolution_WorksWithEmptyList()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>();
            SystemPerformance.LowerResolution(data, 1, 0);
            CollectionAssert.AreEqual(new List<SystemPerformance>(), data.ToList());
        }

        [Category("SystemPerformanceTests")]
        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        public void LowerResolution_WorksWithSingleItemInTheList(int maxPoints, decimal threshold)
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 }
            };

            SystemPerformance.LowerResolution(data, maxPoints, threshold);
            Assert.AreEqual(1, data.Count, "Records were incorrectly removed.");
            AssertPerformanceEquality(10m, new DateTime(2015, 1, 1, 6, 0, 0), data[0]);
        }

        [Category("SystemPerformanceTests")]
        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        public void LowerResolution_WorksWithTwoSameItemsInTheList(int maxPoints, decimal threshold)
        {
            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10 }
            };

            SystemPerformance.LowerResolution(data, maxPoints, threshold);
            Assert.AreEqual(2, data.Count, "Records were incorrectly removed.");
            AssertPerformanceEquality(10m, new DateTime(2015, 1, 1, 6, 0, 0), data[0]);
            AssertPerformanceEquality(10m, new DateTime(2015, 1, 1, 6, 1, 0), data[1]);
        }

        [Category("SystemPerformanceTests")]
        [Test]
        [TestCase(1, 0, 0)] // with zero threshold, all points should be returned
        [TestCase(1, 1, 1)] // with 100% threshold, 2 points should be returned
        [TestCase(2, 0.2, 2)] // with 50% threshold, 3 points should be returned
        [TestCase(4, 1, 3)] // with 100% threshold, 3 points should be returned
        [TestCase(5, 1, 4)] // with zero threshold, all points should be returned
        public void LowerResolution_MaxPointsWorksWithSixItemsInTheList(int maxPoints, decimal threshold, int expectedValuesIndex)
        {
            // These aren't great tests - they're looking at way too much data to test.
            // TODO: Rework this test into smaller tests and manybe use the TestCaseSource
            var expectedValues = new List<List<TimeValue>>
            {
                new List<TimeValue>
                {
                    // all records returned
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.5m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.55m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 13.286m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.611m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 15.341m)
                },
                new List<TimeValue>
                {
                    // 2 records (0 + ends)
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.5m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.55m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 13.286m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.611m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 15.341m)
                },
                new List<TimeValue>
                {
                    // 3 records returned (1 + ends)
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.5m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.55m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 13.286m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.611m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 15.341m)
                },
                new List<TimeValue>
                {
                    // 4 records returned (2 + ends)
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.5m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.55m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 13.286m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.611m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 15.341m)
                },
                new List<TimeValue>
                {
                    // 5 records returned (3 + ends)
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.5m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.55m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 13.286m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.611m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 15.341m)
                },
            };

            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10.5m }, // +5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 11.55m }, // +10%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 3, 0), Value = 13.286m }, // +15%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 4, 0), Value = 14.611m }, // +10%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 5, 0), Value = 15.341m } // +5%
            };

            SystemPerformance.LowerResolution(data, maxPoints, threshold);

            Assert.AreEqual(expectedValues[expectedValuesIndex].Count, data.Count, "Records were incorrectly removed (or added).");
            for (int i = 0; i < expectedValues[expectedValuesIndex].Count; i++)
            {
                var timeValue = expectedValues[expectedValuesIndex].ElementAt(i);
                AssertPerformanceEquality(timeValue.Value, timeValue.DateTime, data[i]);
            }
        }

        [Category("SystemPerformanceTests")]
        [Test]
        // Tests with max 1 item to make sure values are included correctly around the point
        [TestCase(1, 0.025, 0)] // 2.5% threshold
        [TestCase(1, 0.075, 1)] // 7.5% threshold
        public void LowerResolution_ThresholdWorksWithEightPoints(int maxPoints, decimal threshold, int expectedValuesIndex)
        {
            // These aren't great tests - they're looking at way too much data to test.
            // TODO: Rework this test into smaller tests and manybe use the TestCaseSource
            var expectedValues = new List<List<TimeValue>>
            {
                new List<TimeValue>
                {
                    // all records returned
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10.25m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.763m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.57m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 12.727m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 13.682m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.365m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 14.724m)
                },
                new List<TimeValue>
                {
                    // all records returned
                    new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 0, 0), 10.25m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 1, 0), 10.763m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 2, 0), 11.57m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 3, 0), 12.727m),
                    //new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 13.682m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 4, 0), 14.365m),
                    new TimeValue(new DateTime(2015, 1, 1, 6, 5, 0), 14.724m)
                }
            };

            IList<SystemPerformance> data = new List<SystemPerformance>
            {
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10 },
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 0, 0), Value = 10.25m }, // +2.5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 1, 0), Value = 10.763m }, // +5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 2, 0), Value = 11.57m }, // +7.5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 3, 0), Value = 12.727m }, // +10%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 4, 0), Value = 13.682m }, // +7.5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 4, 0), Value = 14.365m }, // +5%
                new SystemPerformance { AuditTime = new DateTime(2015, 1, 1, 6, 5, 0), Value = 14.724m } // +2.5%
            };

            SystemPerformance.LowerResolution(data, maxPoints, threshold);

            Assert.AreEqual(expectedValues[expectedValuesIndex].Count, data.Count, "Records were incorrectly removed (or added).");
            for (int i = 0; i < expectedValues[expectedValuesIndex].Count; i++)
            {
                var timeValue = expectedValues[expectedValuesIndex].ElementAt(i);
                AssertPerformanceEquality(timeValue.Value, timeValue.DateTime, data[i]);
            }
        }

        #endregion Lower Resolution

        #region Methods

        /// <summary>
        /// Helper method that performs an assert on the system performance data point
        /// </summary>
        /// <param name="expectedValue"></param>
        /// <param name="expectedDate"></param>
        /// <param name="dataPoint"></param>
        private static void AssertPerformanceEquality(decimal expectedValue, DateTime expectedDate, SystemPerformance dataPoint)
        {
            Assert.AreEqual(expectedValue, dataPoint.Value, "Performance data point value has changed.");
            Assert.AreEqual(expectedDate, dataPoint.AuditTime, "Data point time has changed.");
        }

        #endregion Methods
    }

    public class TimeValue
    {
        public TimeValue(DateTime dateTime, Decimal value)
        {
            this.DateTime = dateTime;
            this.Value = value;
        }

        public DateTime DateTime { get; set; }
        public Decimal Value { get; set; }
    }
}