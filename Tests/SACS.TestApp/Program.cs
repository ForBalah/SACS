using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SACS.Implementation;
using SACS.Implementation.Execution;

namespace SACS.TestApp
{
    /// <summary>
    /// This test app shows an example of how to setup an app for loading into a Service Application
    /// Container This example is a typical console app which can also be executed in any separate
    /// process such as debugging or as an independent Scheduled Task. For simplicity the ServiceAppBase
    /// is implemented directly on the executable, but in a real app, it could be implemented in a
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
            // only seen when debugging
            Console.WriteLine(ConfigurationManager.AppSettings["Message"]);
            Console.WriteLine(string.Format("Test App ran at {0:F} as user: {1}", DateTime.Now, WindowsIdentity.GetCurrent().Name));

            bool throwEx;
            if (bool.TryParse(ConfigurationManager.AppSettings["ThrowException"], out throwEx) && throwEx)
            {
                throw new TestException("Testing exception throwing");
            }
        }

        protected override void Initialze()
        {
            // can initialize app here
        }
    }
}