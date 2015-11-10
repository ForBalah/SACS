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
        [TestCase("c:\\temp\\test.exe action:'run'")]
        [TestCase("c:\\temp\\test.exe {action: 'run'}")]
        public void Parse_CanReturnSingleJsonParam(string input)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var value = (result.GetCommands())["action"];
            Assert.AreEqual("run", value);
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase("action: 'run'", 0)]
        [TestCase("action: []", 0)]
        [TestCase("action: [ 'run' ]", 1)]
        [TestCase("action: [ 'run', 'stop' ]", 2)]
        [TestCase("action: [ 'run', 'stop' ], display: [ 'version' ]", 2)]
        public void Parse_CanReturnListOfJsonParam(string input, int count)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var value = (result.GetCommands())["action"];
            Assert.AreEqual(count, ((value as object[]) ?? new object[] { }).Length);
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase("{action: 'run'}", 0)]
        [TestCase("c:\\temp\\test.exe action:'run'", 1)]
        [TestCase("c:\\temp\\test.exe exit {action: 'run'}", 2)]
        public void Parse_CanReturnArgsThatAreNotInterpretedAsJson(string input, int expectedArgsCount)
        {
            var processor = new JsonCommandProcessor();
            var result = processor.Parse(input);

            var list = result.GetArgs();
            Assert.AreEqual(expectedArgsCount, list.Count);
        } 
    }
}
