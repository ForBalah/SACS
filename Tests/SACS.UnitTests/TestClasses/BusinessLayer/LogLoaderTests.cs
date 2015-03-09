using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACS.BusinessLayer.BusinessLogic.Logs;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestClass]
    public class LogLoaderTests
    {
        [TestMethod]
        public void LoadLogsFromXml_CanHandleEmptyStrings()
        {
            LogLoader loader = new LogLoader(LogLoader.DefaultDate);
            IList<LogEntry> entries = new List<LogEntry>();
            loader.LoadLogsFromXml(entries, "    ", false);

            Assert.IsTrue(entries.Count == 0);
        }

        [TestMethod]
        public void LoadLogsFromXml_CanProcessAnXmlEntryCorrectlyNoThrowable()
        {
            LogLoader loader = new LogLoader(LogLoader.DefaultDate);
            IList<LogEntry> entries = new List<LogEntry>();
            string xmlData = @"<log4j:event logger=""Topshelf.HostFactory"" timestamp=""1424119928456"" level=""INFO"" thread=""8""><log4j:message>Configuration Result: SACS.Agent</log4j:message><log4j:properties><log4j:data name=""log4jmachinename"" value=""User-PC"" /><log4j:data name=""log4japp"" value=""SACS.WindowsService.vshost.exe"" /><log4j:data name=""log4net:Identity"" value="""" /><log4j:data name=""log4net:UserName"" value=""User-PC\User"" /><log4j:data name=""log4net:HostName"" value=""User-PC"" /></log4j:properties><log4j:locationInfo class=""Topshelf.Logging.Log4NetLogWriter"" method=""InfoFormat"" file="""" line=""0"" /></log4j:event>";

            loader.LoadLogsFromXml(entries, LogLoader.AppendRoot(xmlData), false);

            Assert.AreEqual(1, entries.Count, "LogEntry count");
            Assert.IsTrue(entries[0].TimeStamp > new DateTime(1970, 1, 1), "The timestamp is wrong");
            Assert.AreEqual("INFO", entries[0].Level, "Level value");
            Assert.AreEqual(LogImageType.Info, entries[0].ImageType, "LogImageType value");
            Assert.AreEqual("8", entries[0].Thread, "Thread value");
            Assert.AreEqual("Configuration Result: SACS.Agent", entries[0].Message, "Message");
            Assert.AreEqual("User-PC", entries[0].MachineName, "Machine name");
            Assert.AreEqual("SACS.WindowsService.vshost.exe", entries[0].App, "App");
            Assert.AreEqual("User-PC\\User", entries[0].UserName, "User name");
            Assert.AreEqual("User-PC", entries[0].HostName, "Host name");
            Assert.AreEqual("Topshelf.Logging.Log4NetLogWriter", entries[0].Class, "Class");
            Assert.AreEqual("InfoFormat", entries[0].Method, "Method");
            Assert.AreEqual("", entries[0].File, "File");
            Assert.AreEqual("0", entries[0].Line, "Line");
        }

        [TestMethod]
        public void LoadLogsFromXml_CanProcessAnXmlEntryCorrectlyWithThrowable()
        {
            LogLoader loader = new LogLoader(LogLoader.DefaultDate);
            IList<LogEntry> entries = new List<LogEntry>();
            string xmlData = @"<log4j:event logger=""SACS.BusinessLayer.BusinessLogic.Application.AppManager"" timestamp=""1424119335723"" level=""ERROR"" thread=""8""><log4j:message>EmailService ServiceApp could not be initialized.</log4j:message><log4j:properties><log4j:data name=""log4jmachinename"" value=""User-PC"" /><log4j:data name=""log4japp"" value=""SACS.WindowsService.vshost.exe"" /><log4j:data name=""log4net:Identity"" value="""" /><log4j:data name=""log4net:UserName"" value=""User-PC\User"" /><log4j:data name=""log4net:HostName"" value=""User-PC"" /></log4j:properties><log4j:throwable>System.IO.FileNotFoundException: The system cannot find the file specified. (Exception from HRESULT: 0x80070002)
   at System.Reflection.RuntimeAssembly.nLoadFile(String path, Evidence evidence)
   at System.Reflection.Assembly.LoadFile(String path)
   at SACS.BusinessLayer.BusinessLogic.Loader.DomainInitializer.FindEntryType(String assemblyPath) in e:\Development\SACS\Libraries\SACS.BusinessLayer\BusinessLogic\Loader\DomainInitializer.cs:line 44
   at SACS.BusinessLayer.BusinessLogic.Loader.DomainInitializer.FindEntryType(String assemblyPath)
   at SACS.BusinessLayer.BusinessLogic.Loader.DomainInitializer.GetEntryType(String assemblyPath) in e:\Development\SACS\Libraries\SACS.BusinessLayer\BusinessLogic\Loader\DomainInitializer.cs:line 31
   at SACS.BusinessLayer.BusinessLogic.Domain.ServiceAppDomain.Initialize() in e:\Development\SACS\Libraries\SACS.BusinessLayer\BusinessLogic\Domain\ServiceAppDomain.cs:line 186</log4j:throwable><log4j:locationInfo class=""SACS.BusinessLayer.BusinessLogic.Domain.ServiceAppDomain"" method=""Initialize"" file=""e:\Development\SACS\Libraries\SACS.BusinessLayer\BusinessLogic\Domain\ServiceAppDomain.cs"" line=""198"" /></log4j:event>";

            loader.LoadLogsFromXml(entries, LogLoader.AppendRoot(xmlData), false);

            Assert.AreEqual(1, entries.Count, "LogEntry count");
            Assert.IsTrue(entries[0].TimeStamp > new DateTime(1970, 1, 1), "The timestamp is wrong");
            Assert.AreEqual("ERROR", entries[0].Level, "Level value");
            Assert.AreEqual(LogImageType.Error, entries[0].ImageType, "LogImageType value");
            Assert.AreEqual("8", entries[0].Thread, "Thread value");
            Assert.AreEqual("EmailService ServiceApp could not be initialized.", entries[0].Message, "Message");
            Assert.AreEqual("User-PC", entries[0].MachineName, "Machine name");
            Assert.AreEqual("SACS.WindowsService.vshost.exe", entries[0].App, "App");
            Assert.AreEqual("User-PC\\User", entries[0].UserName, "User name");
            Assert.AreEqual("User-PC", entries[0].HostName, "Host name");
            Assert.IsTrue(entries[0].Throwable.Contains("System.IO.FileNotFoundException:"), "Throwable");
            Assert.AreEqual("SACS.BusinessLayer.BusinessLogic.Domain.ServiceAppDomain", entries[0].Class, "Class");
            Assert.AreEqual("Initialize", entries[0].Method, "Method");
            Assert.AreEqual("e:\\Development\\SACS\\Libraries\\SACS.BusinessLayer\\BusinessLogic\\Domain\\ServiceAppDomain.cs", entries[0].File, "File");
            Assert.AreEqual("198", entries[0].Line, "Line");
        }

        [TestMethod]
        public void LoadLogsFromXml_CanSortDescending()
        {
            LogLoader loader = new LogLoader(LogLoader.DefaultDate);
            IList<LogEntry> entries = new List<LogEntry>();

            // 2 records
            string xmlData = @"<log4j:event logger=""Topshelf.HostFactory"" timestamp=""1424119928456"" level=""INFO"" thread=""8""><log4j:message>Configuration Result: SACS.Agent</log4j:message><log4j:properties><log4j:data name=""log4jmachinename"" value=""User-PC"" /><log4j:data name=""log4japp"" value=""SACS.WindowsService.vshost.exe"" /><log4j:data name=""log4net:Identity"" value="""" /><log4j:data name=""log4net:UserName"" value=""User-PC\User"" /><log4j:data name=""log4net:HostName"" value=""User-PC"" /></log4j:properties><log4j:locationInfo class=""Topshelf.Logging.Log4NetLogWriter"" method=""InfoFormat"" file="""" line=""0"" /></log4j:event>
<log4j:event logger=""Topshelf.HostFactory"" timestamp=""1424119938456"" level=""INFO"" thread=""8""><log4j:message>Configuration Result: SACS.Agent</log4j:message><log4j:properties><log4j:data name=""log4jmachinename"" value=""User-PC"" /><log4j:data name=""log4japp"" value=""SACS.WindowsService.vshost.exe"" /><log4j:data name=""log4net:Identity"" value="""" /><log4j:data name=""log4net:UserName"" value=""User-PC\User"" /><log4j:data name=""log4net:HostName"" value=""User-PC"" /></log4j:properties><log4j:locationInfo class=""Topshelf.Logging.Log4NetLogWriter"" method=""InfoFormat"" file="""" line=""0"" /></log4j:event>";

            // sort descending
            loader.LoadLogsFromXml(entries, LogLoader.AppendRoot(xmlData), true);

            Assert.AreEqual(2, entries.Count, "LogEntry count");
            Assert.AreEqual(2, entries[0].Item);
        }
    }
}
