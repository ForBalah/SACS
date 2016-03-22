using System;
using NSubstitute;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.Factories.Interfaces;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class ServiceAppProcessCollectionTests
    {
        private IServiceAppProcessFactory _serviceAppProcessFactory;

        [SetUp]
        public void InitializeTests()
        {
            var mockServiceApp = (ServiceAppProcess)Activator.CreateInstance(typeof(ServiceAppProcess), true);
            _serviceAppProcessFactory = Substitute.For<IServiceAppProcessFactory>();
            _serviceAppProcessFactory.CreateServiceAppProcess(null, null).ReturnsForAnyArgs(mockServiceApp);
        }

        [Test]
        [Category("ServiceAppProcessCollectionTests")]
        public void GetIndex_CanReturnServiceAppProcessFromCollectionUsingString()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saProc = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);
            collection.Add(saProc);

            Assert.AreEqual(saProc, collection["__Test"]);
        }

        [Test]
        [Category("ServiceAppProcessCollectionTests")]
        public void Add_CanInsertUniqueDomainToCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saProc = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);
            collection.Add(saProc);

            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        [Category("ServiceAppProcessCollectionTests")]
        public void Add_CannotAddDuplicateDomainToCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saProc1 = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);
            ServiceAppProcess saProc2 = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);
            collection.Add(saProc1);

            try
            {
                collection.Add(saProc2);
                Assert.Fail("Able to add duplicate domain.");
            }
            catch (AssertionException)
            {
                throw;
            }
            catch
            {
                Assert.AreEqual(1, collection.Count);
            }
        }

        [Test]
        [Category("ServiceAppProcessCollectionTests")]
        public void Remove_CanRemoveInstalledDomainFromCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saProc = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);
            collection.Add(saProc);

            bool success = collection.Remove(saProc);

            Assert.IsTrue(success);
        }

        [Test]
        [Category("ServiceAppProcessCollectionTests")]
        public void Remove_CannotRemoveDomainFromEmptyCollection()
        {
            ServiceAppProcessCollection collection = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            ServiceAppProcess saProc = _serviceAppProcessFactory.CreateServiceAppProcess(null, null);

            bool success = collection.Remove(saProc);

            Assert.IsFalse(success);
        }
    }
}