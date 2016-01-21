using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Config
{
    /// <summary>
    /// The representation of the Windows management console app.config file.
    /// </summary>
    public class WindowsConsoleConfigFile : ConfigFile
    {
        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void ReloadProperties()
        {
            throw new NotImplementedException();
        }
    }
}