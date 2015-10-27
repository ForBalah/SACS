using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// Class responsible for processing data received from the command line
    /// </summary>
    internal abstract class CommandLineProcessor
    {
        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        internal abstract void Process(string command);

        /// <summary>
        /// Parses the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal abstract IDictionary<string, object> Parse(string command);
    }
}
