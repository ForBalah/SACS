using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// Class responsible for processing data received from the command line.
    /// </summary>
    internal abstract class CommandLineProcessor
    {
        private readonly IList<ICommandHandler> _commandHandlers = new List<ICommandHandler>();

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        internal virtual void Process(string command)
        {
            this.Process(this.Parse(command), false);
            this.Process(this.Parse(command), true);
        }

        internal virtual void ProcessNonActions(CommandObject commandObject)
        {
            this.Process(commandObject, false);
        }

        internal virtual void ProcessActions(CommandObject commandObject)
        {
            this.Process(commandObject, true);
        }

        /// <summary>
        /// Parses the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal abstract CommandObject Parse(string command);

        /// <summary>
        /// Creates a new command processor that will be associated with this <see cref="CommandLineProcessor"/>.
        /// </summary>
        /// <typeparam name="T">The command processor to create.</typeparam>
        /// <param name="args">The constructor args.</param>
        /// <returns>A new instance of a <see cref="ICommandHandler"/>.</returns>
        /// <remarks>This should really go into a builder pattern. However, this works given
        /// the intentions. If a need arises to refactor this into a dedicated builder pattern
        /// then that will be done accordingly.</remarks>
        internal ICommandHandler HoistWith<T>(params object[] args) where T : ICommandHandler
        {
            Type newType = typeof(T);

            ICommandHandler handler = (ICommandHandler)Activator.CreateInstance(
                newType,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                args,
                CultureInfo.InvariantCulture);

            this._commandHandlers.Add(handler);
            return handler;
        }

        /// <summary>
        /// Parses the command line.
        /// </summary>
        /// <param name="leftCommand">The left command.</param>
        /// <param name="commandTree">The command tree.</param>
        protected static void ParseArgs(string leftCommand, Dictionary<string, object> commandTree)
        {
            var separator = new[] { ' ' };
            var splitCommand = leftCommand.Split('"')
                .Select((element, index) => index % 2 == 0 ? //// If even index
                    //// Split the item
                    element.Split(separator, StringSplitOptions.RemoveEmptyEntries) :
                    //// Keep the entire item
                    new string[] { element })
                .SelectMany(element => element).ToList();

            commandTree["args"] = splitCommand;
        }

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        /// <param name="processActionCommands">Indicates whether to process "action" commands or other commands.</param>
        private void Process(CommandObject commandObject, bool processActionCommands)
        {
            var commands = commandObject.GetCommands();
            foreach (var processor in this._commandHandlers.Where(c => c.Type == CommandHandlerType.Command && (!processActionCommands || c.Command == "action")))
            {
                processor.Handle(commands);
            }

            var args = commandObject.GetArgs();
            foreach (var processor in this._commandHandlers.Where(c => c.Type == CommandHandlerType.Args && (!processActionCommands || c.Command == "action")))
            {
                processor.Handle(args);
            }
        }
    }
}