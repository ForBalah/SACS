using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Commands
{
    internal class ArgsHandler : ICommandHandler
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        /// <summary>
        /// Prevents a default instance of the <see cref="ArgsHandler"/> class from being created.
        /// </summary>
        private ArgsHandler()
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
        /// Gets the command handler type.
        /// </summary>
        public CommandHandlerType Type
        {
            get { return CommandHandlerType.Args; }
        }

        /// <summary>
        /// Instructs the command handler to perform the action given the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public ICommandHandler For(string argument, Action action)
        {
            this._actions.Add(argument, action);
            return this;
        }

        /// <summary>
        /// Instructs the command handler to perform the action when the argument matches the type.
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <param name="action">The action to perform on that argument.</param>
        /// <returns></returns>
        public ICommandHandler ForArgs<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        public void Handle(IDictionary<string, object> commandObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        public void Handle(IList<string> argsList)
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