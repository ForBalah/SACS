using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// The command processors.
    /// </summary>
    internal interface ICommandHandler
    {
        /// <summary>
        /// Gets the command that the processor handles.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        string Command { get; }

        /// <summary>
        /// Gets the command handler type.
        /// </summary>
        CommandHandlerType Type { get; }

        /// <summary>
        /// Instructs the command handler to perform the action given the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        ICommandHandler For(string argument, Action action);

        /// <summary>
        /// Instructs the command handler to perform the action when the argument matches the type.
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <param name="action">The action to perform on that argument.</param>
        /// <returns></returns>
        ICommandHandler ForArgs<T>(Action<T> action);

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        void Handle(IDictionary<string, object> commandObject);

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        void Handle(IList<string> argsList);
    }
}