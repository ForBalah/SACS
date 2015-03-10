using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SACS.Implementation;

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
            // For debug purposes this program is executed directly. Since this is the entry point when
            // run the app, we simply create an instance of the class containing the ServiceAppBase implementation
            // ("Program" in this case).
            Program p = new Program();
            p.Initialize();

            // The Execute method is the same method that SACS will call
            p.Execute();

            // Assumes our program is interactive (since we are debugging in this case). You can also check
            // for interactivity if your intention is to use the app in another domain
            Console.ReadKey();

            // because it is good to clean up afterwards. In SACS this only gets called when the app is being
            // stopped.
            p.CleanUp();
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            Console.WriteLine(ConfigurationManager.AppSettings["Message"]);
            Console.WriteLine(string.Format("Test App ran at {0:F} as user: {1}", DateTime.Now, WindowsIdentity.GetCurrent().Name));
            this.SendMessage("Test app has sent this message to the log successfully");

            bool throwException;

            if (bool.TryParse(ConfigurationManager.AppSettings["ThrowException"], out throwException) && throwException)
            {
                throw new InvalidOperationException("Shows what happens when an unhandled exception is thrown");
            }
        }

        /// <summary>
        /// Initializes this ServiceApp implementation.
        /// </summary>
        public override void Initialize()
        {
            // Can initialize your service here
        }

        /// <summary>
        /// Called when the service is being unloaded. this method should contain details on how to free up unmanaged resources.
        /// </summary>
        public override void CleanUp()
        {
        }
    }
}
