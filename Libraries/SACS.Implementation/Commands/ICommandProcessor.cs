using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// The command processors.
    /// </summary>
    internal interface ICommandProcessor
    {
        /// <summary>
        /// Gets the command that the processor handles.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        string Command { get; }

        /// <summary>
        /// Gets the command processor type.
        /// </summary>
        CommandProcessorType Type { get; }

        /// <summary>
        /// Instructs the command processor to perform the action given the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        ICommandProcessor For(string argument, Action action);

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        void Process(IDictionary<string, object> commandObject);

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        void Process(IList<string> argsList);
    }
}