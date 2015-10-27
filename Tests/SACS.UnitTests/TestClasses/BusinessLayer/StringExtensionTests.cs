using System;
using NUnit.Framework;
using SACS.BusinessLayer.Extensions;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
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

        [Test]
        public void Truncate_NoTruncationIfStringIsJustTooShort()
        {
            string result = "hello".Truncate(5);
            Assert.AreEqual("hello", result);
        }

        [Test]
        public void Truncate_TruncatesStringIfItIsTooLong()
        {
            string result = "hello world".Truncate(10, "...");
            Assert.AreEqual("hello worl...", result);
        }
    }
}
