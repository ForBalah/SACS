using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Conversions;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class MhtToHtmlTests
    {
        [Test]
        [Category("MhtToHtml")]
        public void ConvertToSingleHtml_DoesNotCrashOnNull()
        {
            MhtToHtml converter = new MhtToHtml();
            try
            {
                converter.ConvertToHTMLDocument(null);
            }
            catch
            {
                Assert.Fail("Null should be handled.");
            }
        }

        [Test]
        [Category("MhtToHtml")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetBoundary_FailsIfNoBoundaryIsFound()
        {
            MhtToHtml.GetBoundary(null);
        }

        [Test]
        [Category("MhtToHtml")]
        public void GetBoundary_WorksForSingleBoundary()
        {
            string contents =
                "MIME-Version: 1.0" +
                "Content-Type: multipart/related; boundary=\"----=_NextPart_01D185BB.4485E820\"" +
                "This document is a Single File Web Page, also known as a Web Archive file.  If you are seeing this message, your browser or editor doesn't support Web Archive files.  Please download a browser that supports Web Archive, such as Windows� Internet Explorer�.";

            string result = MhtToHtml.GetBoundary(contents);

            Assert.AreEqual("----=_NextPart_01D185BB.4485E820", result);
        }

        [Test]
        [Category("MhtToHtml")]
        public void ContentDetails_ExtractContentDetails_ReturnsFullObject()
        {
            var testString = @"Content-Location: file:///C:/6875A1F0/SACSHelp.htm
                            Content-Transfer-Encoding: quoted-printable
                            Content-Type: text/html; charset=""windows - 1252""";

            var result = MhtToHtml.ContentDetails.ExtractContentDetails(testString);

            Assert.AreEqual("file:///C:/6875A1F0/SACSHelp.htm", result.ContentLocation);
            Assert.AreEqual("quoted-printable", result.ContentTransferEncoding);
            Assert.AreEqual(@"text/html; charset=""windows - 1252""", result.ContentType);
            Assert.IsTrue(result.HasAllContentHeaders);
        }
    }
}
