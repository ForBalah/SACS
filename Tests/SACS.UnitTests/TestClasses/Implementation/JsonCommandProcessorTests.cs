using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SACS.Implementation.Commands;

namespace SACS.UnitTests.TestClasses.Implementation
{
    [TestFixture]
    public class JsonCommandProcessorTests
    {
        [Category("SACS.Implementation.Commands")]
        [TestCase((string)null)]
        [TestCase("")]
        public void Parse_CanReturnAnEmptyObject(string input)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var value = result[""];
            Assert.IsNull(value);
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase("C:\\temp\\assembly.exe")]
        [TestCase("C:\\temp\\assembly.exe asdf")]
        public void Parse_CanReturnSingleItemAsFile(string input)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var value = result["exe"];
            Assert.AreEqual(input, value);
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase("action: 'run'")]
        [TestCase("{action: 'run'}")]
        [TestCase("c:\\temp\\test.exe {action: 'run'}")]
        public void Parse_CanReturnSingleJsonParam(string input)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var value = (result["commands"] as Dictionary<string, object>)["action"];
            Assert.AreEqual("run", value);
        } 
    }
}
