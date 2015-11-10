using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SACS.Implementation.Commands;

namespace SACS.UnitTests.TestClasses.Implementation
{
    [TestFixture]
    public class DirectiveHandlerTests
    {
        private static List<CommandObject> CommandObjects;

        [SetUp]
        public void Setup()
        {
            CommandObjects = new List<CommandObject> 
            {
                new CommandObject { },
                new CommandObject { { "blah", null } },
                new CommandObject { { "commands", new CommandObject { { "test", null } } } },
                new CommandObject { { "commands", new CommandObject { { "action", "test" } } } },
                new CommandObject { { "commands", new CommandObject { { "action", "test" }, { "notaction", "nottest" } } } },
                //// I thought it would be "easier" writing nested dictionaries. Never have I been so wrong @_@
                new CommandObject { { "commands", new CommandObject { { "action", new object[] { } } } } }, 
                new CommandObject { { "commands", new CommandObject { { "action", new object[] { "blah" } } } } },
                new CommandObject { { "commands", new CommandObject { { "action", new object[] { "test" } } } } },
                new CommandObject { { "commands", new CommandObject { { "action", new object[] { "blah", "test" } } } } },
            };
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        public void Handle_WorksForSingleArg(int commandObjectIndex, bool shouldBeCalled)
        {
            bool isCalled = false;
            var handler = GetHandler<DirectiveHandler>("action");
            handler.For("test", () => isCalled = true);

            var commandObject = CommandObjects[commandObjectIndex];
            handler.Handle(commandObject.GetCommands());

            Assert.AreEqual(shouldBeCalled, isCalled);
        }

        [Category("SACS.Implementation.Commands")]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        public void Handle_WorksForListOfArgs(int commandObjectIndex, bool shouldBeCalled)
        {
            bool isCalled = false;
            var handler = GetHandler<DirectiveHandler>("action");
            handler.For("test", () => isCalled = true);

            var commandObject = CommandObjects[commandObjectIndex];
            handler.Handle(commandObject.GetCommands());

            Assert.AreEqual(shouldBeCalled, isCalled);
        }

        private ICommandHandler GetHandler<T>(string commandName) where T : ICommandHandler
        {
            Type newType = typeof(T);

            ICommandHandler handler = (ICommandHandler)Activator.CreateInstance(
                newType,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { commandName },
                CultureInfo.InvariantCulture);
            return handler;
        }
    }
}
