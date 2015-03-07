using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACS.BusinessLayer.Extensions;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void Truncate_DoesNotBreakWhenGivenNull()
        {
            string nullString = null;

            try
            {
                nullString.Truncate(0);
            }
            catch
            {
                Assert.Fail("Truncate failed on NULL string");
            }
        }

        [TestMethod]
        public void Truncate_NoTruncationIfStringIsJustTooShort()
        {
            string result = "hello".Truncate(5);
            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void Truncate_TruncatesStringIfItIsTooLong()
        {
            string result = "hello world".Truncate(10, "...");
            Assert.AreEqual("hello worl...", result);
        }
    }
}
