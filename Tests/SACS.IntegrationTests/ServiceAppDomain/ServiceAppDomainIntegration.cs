using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain = SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.DataAccessLayer.Models;
using log4net;
using NSubstitute;
using System.Collections.Generic;
using SACS.Common.Enums;
using System.Threading;

namespace SACS.IntegrationTests.ServiceAppDomain
{
    [TestClass]
    public class ServiceAppDomainIntegration
    {
        private List<Domain.ServiceAppDomain> domains = new List<Domain.ServiceAppDomain>();

        [TestMethod]
        public void ServiceAppDomainCanInitializeNewAppDomain()
        {
            Domain.ServiceAppDomain domain = InitializeDomain();
            Assert.AreEqual(ServiceAppState.Initialized, domain.CurrentState);
        }

        [TestMethod]
        public void ServiceAppDomainCanUnloadAppDomain()
        {
            Domain.ServiceAppDomain domain = InitializeDomain();
            domain.Unload();

            // this is a timed test. The domain must unload in a certain time frame else it is an error
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            int tries = 5;
            bool? success = null;

            timer.Elapsed += (sender, e) =>
            {
                if (domain.CurrentState == ServiceAppState.NotLoaded)
                {
                    success = true;
                    timer.Stop();
                }
                else
                { 
                    tries--;
                    if (tries <= 0)
                    {
                        success = false;
                    }
                }
            };
            timer.Start();

            Thread.Sleep(1000 * (tries + 1));

            if (success.HasValue)
            {
                if (success.Value)
                {
                    Assert.IsTrue(domain.CurrentState == ServiceAppState.NotLoaded);
                }
                else
                {
                    Assert.Fail("ServiceAppDomain failed to unload");
                }
            }
            else
            {
                Assert.Inconclusive("ServiceAppDomain state is unknown");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            domains.ForEach(d =>
            {
                d.Unload();
            });
        }

        private Domain.ServiceAppDomain InitializeDomain()
        {
            ILog log = Substitute.For<ILog>();
            ServiceApp app = new ServiceApp
            {
                Name = "Test",
                Description = "Test description",
                //// using the same assembly and path in this project
                Assembly = Assembly.GetExecutingAssembly().FullName,
                EntryFile = Assembly.GetExecutingAssembly().Location,
                Path = System.Environment.CurrentDirectory,
                //// must follow folder structure of config
                ConfigFilePath = System.Environment.CurrentDirectory + "\\ServiceAppDomain\\TestService.config",
                Environment = "Integration Test"
            };

            Domain.ServiceAppDomain domain = new Domain.ServiceAppDomain(app, log, new BusinessLayer.BusinessLogic.Security.ServiceAppImpersonator());
            domains.Add(domain);
            domain.Initialize();

            return domain;
        }
    }
}
