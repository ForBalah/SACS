using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// The command processor type.
    /// </summary>
    internal enum CommandHandlerType
    {
        /// <summary>
        /// No processor type specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the "commands" processor type.
        /// </summary>
        Command,

        /// <summary>
        /// Represents the "args" processor type.
        /// </summary>
        Args
    }
}