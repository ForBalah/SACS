using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NSubstitute;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Errors;
using SACS.Common.Factories.Interfaces;
using SACS.Common.Helpers;
using SACS.Common.Runtime;
using SACS.DataAccessLayer.Models;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class ServiceAppProcessTests
    {
        #region Common fields/properties
        #endregion

        [Test]
        [Category("ServiceAppProcess")]
        public void Constructor_ServiceAppProcessConstructorHasNoSurprises()
        {
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());

            ServiceAppProcess appProcToTest = new ServiceAppProcess(
                new ServiceApp { Name = "Test" },
                Substitute.For<ILog>(), 
                procFactory);
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void Start_ServiceAppProcessCanStartSucessfully()
        {
            ILog log = Substitute.For<ILog>();
            bool monitorMethodCalled = false;

            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());

            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);
            appProcToTest.MonitorProcessCallback = () => { monitorMethodCalled = true; };

            Task t = appProcToTest.Start();
            bool success = t.Wait(500);

            if (!success)
            {
                Assert.Fail("Start() took too long to finish");
            }

            log.DidNotReceive().Error(Arg.Any<object>(), Arg.Any<Exception>());
            log.DidNotReceive().Warn(Arg.Any<object>(), Arg.Any<Exception>());
            Assert.IsTrue(monitorMethodCalled, "MonitorProcessCallback() not called.");
        }

        /* This will need to come after all ProcessMessage tests
        [Test]
        [Category("ServiceAppProcess")]
        public void Stop_ServiceAppProcessCanStopWithoutIssues()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);
        }
        */

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessMessage_DebugIsLogged()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);

            bool shouldExit = appProcToTest.ProcessMessage("{debug:\"test\"}");

            log.Received().Debug("From Test: test");
            Assert.IsFalse(shouldExit, "Process message was not meant to return true");
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessMessage_InfoIsLogged()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);

            bool shouldExit = appProcToTest.ProcessMessage("{info:\"test\"}");

            log.Received().Info("test");
            Assert.IsFalse(shouldExit, "Process message was not meant to return true");
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessMessage_InfoAsANormalStringIsLogged()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);

            bool shouldExit = appProcToTest.ProcessMessage("test string");

            log.Received().Info("test string");
            Assert.IsFalse(shouldExit, "Process message was not meant to return true");
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessError_LoggingWorksWithRecognizedExceptionObject()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);
            Exception testException = new InvalidOperationException("test");

            string exceptionMessage = string.Format(@"{{ error: {{
                    details: {{
                        type: ""{0}"",
                        message: ""{1}"",
                        source: ""{2}"",
                        stackTrace: ""{3}""
                    }},
                    exception: ""{4}""
                }} }}",
                testException.GetType().ToString(),
                testException.Message,
                testException.Source,
                testException.StackTrace,
                ExceptionHelper.ConvertToBase64(testException));

            bool shouldExit = appProcToTest.ProcessMessage(exceptionMessage);

            log.Received().Warn(Arg.Any<string>(), Arg.Any<InvalidOperationException>());
            Assert.IsFalse(shouldExit, "Process message was not meant to return true");
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessError_LoggingWorksWithUnrecognizedExceptionObject()
        {
            ILog log = Substitute.For<ILog>();
            IProcessWrapperFactory procFactory = Substitute.For<IProcessWrapperFactory>();
            procFactory.CreateProcess().Returns(new FakeStandardProcess());
            var serviceApp = new ServiceApp { Name = "Test", AppVersion = new Version() };
            ServiceAppProcess appProcToTest = new ServiceAppProcess(serviceApp, log, procFactory);
            Exception testException = new InvalidOperationException("test");

            string exceptionMessage = string.Format(@"{{ error: {{
                    details: {{
                        type: ""{0}"",
                        message: ""{1}"",
                        source: ""{2}"",
                        stackTrace: ""{3}""
                    }},
                    exception: ""xxxxx""
                }} }}",
                testException.GetType().ToString(),
                testException.Message,
                testException.Source,
                testException.StackTrace);

            bool shouldExit = appProcToTest.ProcessMessage(exceptionMessage);

            log.Received().Warn(Arg.Any<string>(), Arg.Any<CustomException>());
            Assert.IsFalse(shouldExit, "Process message was not meant to return true");
        }

        /*
        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessPerformance_PerformanceObjectIsValid()
        {
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessState_StartedWorksAndEventRaised()
        {
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessState_ExecutingWorksAndEventRaised()
        {
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessState_IdleWorksAndEventRaised()
        {
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessState_StoppedWorksAndEventRaised()
        {
        }

        [Test]
        [Category("ServiceAppProcess")]
        public void ProcessState_FailedWorksAndEventRaised()
        {
        }
        */

        #region Helper and mock classes

        private class FakeStandardProcess : ProcessWrapper
        {
            MockStreamReader stdOut;
            MockStreamWriter stdIn;

            public FakeStandardProcess() : base(false)
            {
                stdOut = new MockStreamReader(new MemoryStream());
                stdIn = new MockStreamWriter(new MemoryStream());

                stdOut.InputReceived += StdOut_InputReceived;
                stdIn.OutputReceived += StdIn_OutputReceived;
            }

            public override StreamReader StandardOutput
            {
                get
                {
                    return stdOut;
                }
            }

            public override StreamWriter StandardInput
            {
                get
                {
                    return stdIn;
                }
            }

            public Action<string> StandardInputAction
            {
                get; set;
            }

            public Action<string> StandardOutputAction
            {
                get; set;
            }

            public override bool Start()
            {
                return true;
            }

            private void StdOut_InputReceived(object sender, string e)
            {
                if (StandardInputAction != null)
                {
                    StandardInputAction(e);
                }
            }

            private void StdIn_OutputReceived(object sender, string e)
            {
                if (StandardOutputAction != null)
                {
                    StandardOutputAction(e);
                }
            }
        }

        private class MockStreamReader : StreamReader
        {
            public event EventHandler<string> InputReceived;

            public MockStreamReader(Stream stream) : base(stream)
            {
            }

            public override string ReadLine()
            {
                string result = base.ReadLine();
                if (InputReceived != null)
                {
                    InputReceived(this, result);
                }

                return result;
            }
        }

        private class MockStreamWriter : StreamWriter
        {
            public event EventHandler<string> OutputReceived;

            public MockStreamWriter(Stream stream) : base(stream)
            {
            }

            public override void WriteLine()
            {
                WriteLine(string.Empty);
            }

            public override void WriteLine(string value)
            {
                if (OutputReceived != null)
                {
                    OutputReceived(this, value);
                }

                base.WriteLine(value);
            }
        }

        #endregion
    }
}
