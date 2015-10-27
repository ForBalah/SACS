using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class LogSearchCriteriaTests
    {
        private IList<LogEntry> EmptyList = new List<LogEntry>();

        private IList<LogEntry> SingleList = new List<LogEntry> { new LogEntry { Item = 1, Message = "a" } };

        private IList<LogEntry> MultiList = new List<LogEntry>
        {
            new LogEntry { Item = 1, Message = "a" },
            new LogEntry { Item = 2, Message = "ab" },
            new LogEntry { Item = 3, Message = "c" },
            new LogEntry { Item = 4, Message = "d" },
            new LogEntry { Item = 5, Message = "ef" }
        };

        [Test]
        public void FilterLogs_EmptySearchCriteriaReturnsOriginalList()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
