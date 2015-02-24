using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.DataAccessLayer
{
    [TestClass]
    public class AppListDaoTests
    {
        private string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<appList>
  <collection>
    <serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         path=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug""
         entryFile=""SACS.TestApp.exe""
         assembly=""SACS.TestApp""
         startupType=""Automatic""
         configFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config""
         schedule=""* * * * *""></serviceApp>
    <serviceApp name=""TestApp2""
         description=""Shows how SACS works with multiple of the same app""
         environment=""Development""
         path=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug""
         entryFile=""SACS.TestApp.exe""
         assembly=""SACS.TestApp""
         startupType=""Disabled""
         configFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config""
         schedule=""* * * * *""
         username=""username""
         password=""password""></serviceApp>
  </collection>
</appList>";

        private string serviceAppWithoutIdentityXml = @"<serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         path=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug""
         entryFile=""SACS.TestApp.exe""
         assembly=""SACS.TestApp""
         startupType=""Automatic""
         configFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config""
         schedule=""* * * * *""></serviceApp>";

        private string serviceAppWithIdentityXml = @"<serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         path=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug""
         entryFile=""SACS.TestApp.exe""
         assembly=""SACS.TestApp""
         startupType=""Automatic""
         configFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config""
         schedule=""* * * * *""
         username=""username""
         password=""password""></serviceApp>";

        [TestMethod]
        public void FindAll_CanReturnAllServiceApps()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            var apps = dao.FindAll();

            Assert.AreEqual(2, apps.Count());
        }

        [TestMethod]
        public void FindAll_CanReturnServiceAppByName()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            var app = dao.FindAll(a => a.Name == "TestApp1").FirstOrDefault();

            Assert.AreEqual("TestApp1", app.Name);
            Assert.AreEqual("Shows how SACS works", app.Description);
            Assert.AreEqual("Development", app.Environment);
            Assert.AreEqual(@"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug", app.Path);
            Assert.AreEqual("SACS.TestApp", app.Assembly);
            Assert.AreEqual("SACS.TestApp.exe", app.EntryFile);
            Assert.AreEqual(StartupType.Automatic, app.StartupTypeEnum);
            Assert.AreEqual(@"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config", app.ConfigFilePath);
            Assert.AreEqual("* * * * *", app.Schedule);
            Assert.AreEqual("", app.Username);
            Assert.AreEqual("", app.Password);
        }

        [TestMethod]
        public void CastToServiceApp_CanCastWithoutIdentity()
        {
            XElement serviceAppXml = XElement.Parse(serviceAppWithoutIdentityXml);
            try
            {
                var app = AppListDao.CastToServiceApp(serviceAppXml);
                Assert.AreEqual("", app.Username);
                Assert.AreEqual("", app.Password);
            }
            catch
            {
                Assert.Fail("Failed to cast XML without identity");
            }
        }

        [TestMethod]
        public void CastToServiceApp_CanCastWithIdentity()
        {
            XElement serviceAppXml = XElement.Parse(serviceAppWithIdentityXml);
            try
            {
                var app = AppListDao.CastToServiceApp(serviceAppXml);
                Assert.AreEqual("username", app.Username);
                Assert.AreEqual("password", app.Password);
            }
            catch
            {
                Assert.Fail("Failed to cast XML with identity");
            }
        }

        [TestMethod]
        public void PersistServiceApp_CanAddUserNameAndPassword()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            ServiceApp app = new ServiceApp
            {
                 Name = "TestApp1",
                 Description = "Shows how SACS works",
                 Environment = "Development",
                 Path = @"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug",
                 EntryFile = "SACS.TestApp.exe",
                 Assembly = "SACS.TestApp",
                 StartupTypeEnum = StartupType.Automatic,
                 ConfigFilePath = @"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe.config",
                 Schedule= "* * * * *",
                 Username = "NewUsername",
                 Password = "NewPassword"
            };

            dao.PersistServiceApp(app);

            ServiceApp savedApp = dao.FindAll(sa => sa.Name == "TestApp1").FirstOrDefault();
            Assert.AreEqual("NewUsername", savedApp.Username);
            Assert.AreEqual("NewPassword", savedApp.Password);
        }
    }
}
