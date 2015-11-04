using System;
using NUnit.Framework;
using NSubstitute;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Security;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class ServiceAppProcessCollectionTests
    {
        private log4net.ILog _log;

        [SetUp]
        public void InitializeTests()
        {
            _log = Substitute.For<log4net.ILog>();
        }

        [Test]
        public void GetIndex_CanReturnServiceAppProcessFromCollectionUsingString()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saDomain = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);
            collection.Add(saDomain);

            Assert.AreEqual(saDomain, collection["Test"]);
        }

        [Test]
        public void Add_CanInsertUniqueDomainToCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saDomain = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);
            collection.Add(saDomain);

            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void Add_CannotAddDuplicateDomainToCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saDomain1 = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);
            ServiceAppProcess saDomain2 = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);
            collection.Add(saDomain1);

            try
            {
                collection.Add(saDomain2);
                Assert.Fail("Able to add duplicate domain.");
            }
            catch (Exception)
            {
                Assert.AreEqual(1, collection.Count);
            }
        }

        [Test]
        public void Remove_CanRemoveInstalledDomainFromCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saDomain = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);
            collection.Add(saDomain);

            bool success = collection.Remove(saDomain);

            Assert.IsTrue(success);
        }

        [Test]
        public void Remove_CannotRemoveDomainFromEmptyCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saDomain = new ServiceAppProcess(new ServiceApp() { Name = "Test" }, _log);

            bool success = collection.Remove(saDomain);

            Assert.IsFalse(success);
        }
    }
}
