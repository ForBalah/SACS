using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Export;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class ServiceAppListExporterTests
    {
        [Test]
        public void ExportCsv_CanExportCsvHeadersOnly()
        {
            ServiceAppListExporter exporter = new ServiceAppListExporter();
            string result = exporter.ExportCsv(new List<ServiceApp>());

            // a weird test considering that the properties order can change but still be perfectly valid.
            Assert.IsTrue(result.Contains("Name,"));
        }

        [Test]
        public void ExportCsv_CanExportCsvWithDetails()
        {
            var list = new List<ServiceApp>
            {
                new ServiceApp
                {
                    Name = "Number1",
                    Description = "This \"Description\", complex",
                    Path = "C:\\Path",
                    EntryFile = "EntryFile"
                }
            };

            ServiceAppListExporter exporter = new ServiceAppListExporter();
            string result = exporter.ExportCsv(list);

            string[] lines = Regex.Split(result, "\r\n|\r|\n");

            Assert.IsTrue(lines.Length == 3, "Number of lines");
            Assert.IsTrue(result.Contains("Number1"), "The name");
            Assert.IsTrue(result.Contains("\"This \"\"Description\"\", complex\""), "The quotes and commas");
        }
    }
}
