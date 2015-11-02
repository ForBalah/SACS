using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SACS.Implementation;
using SACS.Implementation.Execution;
using SACS.Implementation.Utils;

namespace SACS.UnitTests.TestClasses.Implementation
{
    [TestFixture]
    [Category("SACS.Implementation")]
    public class ServiceAppBaseTests
    {
        [Test]
        public void Start_WillBeAbleToStartAVanillaServiceApp()
        {
            var fakeMessageProvider = Substitute.For<MessageProvider>();
            fakeMessageProvider.SerializeAsInfo(string.Empty).ReturnsForAnyArgs("started");
            Messages.Provider = fakeMessageProvider;

            bool initializedSuccessfully = false;
            var app = new FakeServiceApp();
            app.InitializeResolver = () => { initializedSuccessfully = true; };
            app.AwaitcommandResolver = () => { /* do nothing */ };

            app.Start();

            fakeMessageProvider.ReceivedWithAnyArgs().SerializeAsInfo(string.Empty);
            Assert.IsTrue(initializedSuccessfully);
        }

        [Test]
        public void QueueExecution_CanQueueIdempotentExecutionCorrectly()
        {

        }

        class FakeServiceApp : ServiceAppBase
        {
            public Action<ServiceAppContext> ExecuteResolver { get; set; }
            public Action InitializeResolver { get; set; }
            public Action AwaitcommandResolver { get; set; }

            public override void Execute(ref ServiceAppContext context)
            {
                ExecuteResolver(context);
            }

            protected override void Initialze()
            {
                InitializeResolver();
            }

            internal override void AwaitCommand()
            {
                AwaitcommandResolver();
            }
        }
    }
}
