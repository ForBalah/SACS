using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.DataAccessLayer
{
    [TestFixture]
    public class AppListDaoTests
    {
        private string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<appList>
  <collection>
    <serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         appFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe""
         startupType=""Automatic""
         schedule=""* * * * *""></serviceApp>
    <serviceApp name=""TestApp2""
         description=""Shows how SACS works with multiple of the same app""
         environment=""Development""
         appFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe""
         startupType=""Disabled""
         schedule=""* * * * *""
         username=""username""
         password=""password""></serviceApp>
  </collection>
</appList>";

        private string serviceAppWithoutIdentityXml = @"<serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         appFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe""
         startupType=""Automatic""
         schedule=""* * * * *""></serviceApp>";

        private string serviceAppWithIdentityXml = @"<serviceApp name=""TestApp1""
         description=""Shows how SACS works""
         environment=""Development""
         appFilePath=""E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe""
         startupType=""Automatic""
         schedule=""* * * * *""
         username=""username""
         password=""password""></serviceApp>";

        [Test]
        public void FindAll_CanReturnAllServiceApps()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            var apps = dao.FindAll();

            Assert.AreEqual(2, apps.Count());
        }

        [Test]
        public void FindAll_CanReturnServiceAppByName()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            var app = dao.FindAll(a => a.Name == "TestApp1").FirstOrDefault();

            Assert.AreEqual("TestApp1", app.Name);
            Assert.AreEqual("Shows how SACS works", app.Description);
            Assert.AreEqual("Development", app.Environment);
            Assert.AreEqual(@"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe", app.AppFilePath);
            Assert.AreEqual(StartupType.Automatic, app.StartupTypeEnum);
            Assert.AreEqual("* * * * *", app.Schedule);
            Assert.AreEqual("", app.Username);
            Assert.AreEqual("", app.Password);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void PersistServiceApp_CanAddUserNameAndPassword()
        {
            IAppListDao dao = DaoFactory.Create<IAppListDao, AppListDao>(xml);
            ServiceApp app = new ServiceApp
            {
                Name = "TestApp1",
                Description = "Shows how SACS works",
                Environment = "Development",
                AppFilePath = @"E:\Development\SACS\Tests\SACS.TestApp\bin\Debug\SACS.TestApp.exe",
                StartupTypeEnum = StartupType.Automatic,
                Schedule = "* * * * *",
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