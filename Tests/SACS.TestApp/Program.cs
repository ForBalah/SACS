using System;
using System.Configuration;
using System.Text;
using System.Threading;
using SACS.Implementation;
using SACS.Implementation.Execution;

namespace SACS.TestApp
{
    /// <summary>
    /// This test app shows an example of how to setup an app for loading into a Service Application
    /// Container This example is a typical console app which can also be executed in any separate
    /// process such as debugging or as an independent Scheduled Task. For simplicity the ServiceAppBase
    /// is implemented directly on the executable class, but in a real app, it could be implemented in a
    /// separate class.
    /// </summary>
    public class Program : ServiceAppBase
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Program p = new Program();
            p.Start(ExecutionMode.Idempotent);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute(ref ServiceAppContext context)
        {
            // Will write to the console when in debug mode, and will write to the SAC that it is hosted in.
            Console.WriteLine(
                "User {0}. Message: {1}. Parameters: {2}", 
                Environment.UserName, 
                ConfigurationManager.AppSettings["Message"],
                Implementation.Utils.Settings.Parameters);

            bool throwEx;
            if (bool.TryParse(ConfigurationManager.AppSettings["ThrowException"], out throwEx) && throwEx)
            {
                throw new TestException("Testing exception throwing");
            }

            bool testMemory;
            if (bool.TryParse(ConfigurationManager.AppSettings["TestMemory"], out testMemory) && testMemory)
            {
                Random rand = new Random();
                StringBuilder builder = new StringBuilder();

                for (long i = 0; i < 250 * 1000L * 1000L; i++)
                {
                    builder.Append((char)rand.Next(32, 127));
                }
            }

            Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["SleepSeconds"]) * 1000);
        }

        protected override void Initialize()
        {
            // can initialize app here
        }
    }
}