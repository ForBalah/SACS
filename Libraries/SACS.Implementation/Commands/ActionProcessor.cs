using System;
using System.Collections.Generic;

namespace SACS.Implementation.Commands
{
    internal class ActionProcessor : ICommandProcessor
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        /// <summary>
        /// Prevents a default instance of the <see cref="ActionProcessor"/> class from being created.
        /// </summary>
        private ActionProcessor()
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
            get { return "action"; }
        }

        /// <summary>
        /// Gets the command processor type.
        /// </summary>
        public CommandProcessorType Type
        {
            get { return CommandProcessorType.Command; }
        }

        /// <summary>
        /// Instructs the command processor to perform the action given the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        /// <remarks>This should really go into a builder pattern. However, this works given
        /// the intentions. If a need arises to refactor this into a dedicated builder pattern
        /// then that will be done accordingly.</remarks>
        public ICommandProcessor For(string argument, Action action)
        {
            this._actions[argument] = action;
            return this;
        }

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        public void Process(IDictionary<string, object> commandObject)
        {
            object actionCommand;

            if (commandObject.TryGetValue(this.Command, out actionCommand))
            {
                string instruction = actionCommand as string;

                if (this._actions.ContainsKey(instruction))
                {
                    // perform the action
                    this._actions[instruction]();
                }

                // we are done processing it
                commandObject.Remove(this.Command);
            }
        }

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        /// <exception cref="System.NotImplementedException">Action processor cannot process args.</exception>
        public void Process(IList<string> argsList)
        {
            throw new NotImplementedException();
        }
    }
}