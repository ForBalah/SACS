using System;
using System.Collections.Generic;
using NUnit.Framework;
using SACS.DataAccessLayer.Models;
using System.Linq;

namespace SACS.UnitTests.TestClasses.DataAccessLayer
{
    [TestFixture]
    public class SystemPerformanceTests
    {
        private const int secondsInterval = 90;

        [Test]
        public void CompactData_WorksWithEmptyList()
        {
            IList<SystemPerformance> data = new List<SystemPerformance>();
            SystemPerformance.CompactData(data, secondsInterval);
            CollectionAssert.AreEqual(new List<SystemPerformance>(), data.ToList());
        }

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
    }
}
