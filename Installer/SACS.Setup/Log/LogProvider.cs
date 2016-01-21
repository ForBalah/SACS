using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Log
{
    /// <summary>
    /// The class that performs the logging
    /// </summary>
    public abstract class LogProvider
    {
        public abstract void Log(Type type, string message, Exception e);

        public abstract void OpenLog();
    }
}