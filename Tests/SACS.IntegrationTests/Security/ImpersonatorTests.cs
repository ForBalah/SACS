using System;
using System.ComponentModel;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Security;

namespace SACS.IntegrationTests.Security
{
    [TestFixture]
    public class ImpersonatorTests
    {
        [Test]
        public void RunAs_CheckThatAWrongUserDoesNotCrashTheSystem()
        {
            ServiceAppImpersonator impersonator = new ServiceAppImpersonator();

            try
            {
                impersonator.RunAsUser("wrongUser", "gPNkPi/pw6E/KrzTpk38+w==", () => Assert.Fail("This action was not supposed to run"));
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(Win32Exception), e.GetType());
            }
        }
    }
}
