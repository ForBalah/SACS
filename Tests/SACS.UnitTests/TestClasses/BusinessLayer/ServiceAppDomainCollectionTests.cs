using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Security;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestClass]
    public class ServiceAppDomainCollectionTests
    {
        private log4net.ILog _log;

        [TestInitialize]
        public void InitializeTests()
        {
            _log = Substitute.For<log4net.ILog>();
        }

        [TestMethod]
        public void GetIndex_CanReturnServiceAppDomainFromCollectionUsingString()
        {
            ServiceAppDomainCollection collection = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
            ServiceAppDomain saDomain = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());
            collection.Add(saDomain);

            Assert.AreEqual(saDomain, collection["Test"]);
        }

        [TestMethod]
        public void Add_CanInsertUniqueDomainToCollection()
        {
            ServiceAppDomainCollection collection = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
            ServiceAppDomain saDomain = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());
            collection.Add(saDomain);

            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void Add_CannotAddDuplicateDomainToCollection()
        {
            ServiceAppDomainCollection collection = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
            ServiceAppDomain saDomain1 = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());
            ServiceAppDomain saDomain2 = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());
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

        [TestMethod]
        public void Remove_CanRemoveInstalledDomainFromCollection()
        {
            ServiceAppDomainCollection collection = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
            ServiceAppDomain saDomain = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());
            collection.Add(saDomain);

            bool success = collection.Remove(saDomain);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Remove_CannotRemoveDomainFromEmptyCollection()
        {
            ServiceAppDomainCollection collection = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
            ServiceAppDomain saDomain = new ServiceAppDomain(new ServiceApp() { Name = "Test" }, _log, new ServiceAppImpersonator());

            bool success = collection.Remove(saDomain);

            Assert.IsFalse(success);
        }
    }
}
