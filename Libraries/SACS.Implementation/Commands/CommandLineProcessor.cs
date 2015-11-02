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
        private readonly IList<ICommandProcessor> _commandProcessors = new List<ICommandProcessor>();

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        internal virtual void Process(string command)
        {
            var commandObject = this.Parse(command);

            if (commandObject.ContainsKey("commands"))
            {
                var commands = commandObject["commands"] as Dictionary<string, object>;
                foreach (var processor in this._commandProcessors.Where(c => c.Type == CommandProcessorType.Command))
                {
                    processor.Process(commands);
                }
            }

            if (commandObject.ContainsKey("args"))
            {
                var args = commandObject["args"] as List<string>;
                foreach (var processor in this._commandProcessors.Where(c => c.Type == CommandProcessorType.Args))
                {
                    processor.Process(args);
                }
            }
        }

        /// <summary>
        /// Parses the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal abstract IDictionary<string, object> Parse(string command);

        /// <summary>
        /// Creates a new command processor that will be associated with this <see cref="CommandLineProcessor"/>.
        /// </summary>
        /// <typeparam name="ICommandProcessor">The command processor to create.</typeparam>
        /// <param name="args">The constructor args.</param>
        /// <returns>A new instance of a <see cref="ICommandProcessor"/>.</returns>
        /// <remarks>This should really go into a builder pattern. However, this works given
        /// the intentions. If a need arises to refactor this into a dedicated builder pattern
        /// then that will be done accordingly.</remarks>
        internal ICommandProcessor HoistWith<T>(params object[] args) where T : ICommandProcessor
        {
            Type newType = typeof(T);

            ICommandProcessor processor = (ICommandProcessor)Activator.CreateInstance(newType, true);
            this._commandProcessors.Add(processor);
            return processor;
        }

        /// <summary>
        /// Parses the command line.
        /// </summary>
        /// <param name="leftCommand">The left command.</param>
        /// <param name="commandTree">The command tree.</param>
        protected static void ParseArgs(string leftCommand, Dictionary<string, object> commandTree)
        {
            var splitCommand = leftCommand.Split('"')
                .Select((element, index) => index % 2 == 0 ?// If even index
                    // Split the item
                    element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) :
                    // Keep the entire item
                    new string[] { element })
                .SelectMany(element => element).ToList();

            commandTree["args"] = splitCommand;
        }
    }
}
