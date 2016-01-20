using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SACS.Setup
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetupForm mainForm = new SetupForm();

            Application.ThreadException += mainForm.Application_UIThreadException;
            AppDomain.CurrentDomain.UnhandledException += mainForm.CurrentDomain_UnhandledException;

            Application.Run(mainForm);
        }
    }
}