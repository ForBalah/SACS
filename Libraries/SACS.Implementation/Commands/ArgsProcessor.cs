using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Commands
{
    internal class ArgsProcessor : ICommandProcessor
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        /// <summary>
        /// Prevents a default instance of the <see cref="ArgsProcessor"/> class from being created.
        /// </summary>
        private ArgsProcessor()
        {
        }

        /// <summary>
        /// Gets the command that the processor handles.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public string Command
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the command processor type.
        /// </summary>
        public CommandProcessorType Type
        {
            get { return CommandProcessorType.Args; }
        }

        /// <summary>
        /// Instructs the command processor to perform the action given the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public ICommandProcessor For(string argument, Action action)
        {
            this._actions.Add(argument, action);
            return this;
        }

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        public void Process(IDictionary<string, object> commandObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        public void Process(IList<string> argsList)
        {
            foreach (var arg in argsList)
            {
                if (this._actions.ContainsKey(arg))
                {
                    this._actions[arg]();
                }
            }
        }
    }
}