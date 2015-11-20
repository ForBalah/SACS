using System;
using System.Collections.Generic;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// Provides a means of processing commands, prefixed with "action"
    /// </summary>
    internal class DirectiveHandler : ICommandHandler
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        /// <summary>
        /// Prevents a default instance of the <see cref="DirectiveHandler"/> class from being created.
        /// </summary>
        /// <param name="directive">The name of the directive</param>
        private DirectiveHandler(string directive)
        {
            this.Command = directive;
        }

        /// <summary>
        /// Gets the command that the processor handles.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public string Command
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command processor type.
        /// </summary>
        public CommandHandlerType Type
        {
            get { return CommandHandlerType.Command; }
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
        public ICommandHandler For(string argument, Action action)
        {
            this._actions[argument] = action;
            return this;
        }

        /// <summary>
        /// Processes the specified command object.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        public void Handle(IDictionary<string, object> commandObject)
        {
            object actionCommand;

            if (commandObject.TryGetValue(this.Command, out actionCommand))
            {
                this.HandleSingleDirective(actionCommand);
                this.HandleDirectiveList(actionCommand);

                // we are done processing it
                commandObject.Remove(this.Command);
            }
        }

        /// <summary>
        /// Handles the directive list.
        /// </summary>
        /// <param name="actionCommand">The action command.</param>
        private void HandleDirectiveList(object actionCommand)
        {
            IEnumerable<object> instructions = actionCommand as IEnumerable<object>;

            if (instructions != null)
            {
                foreach (var instruction in instructions)
                {
                    var stringInstruction = instruction as string;
                    if (this._actions.ContainsKey(stringInstruction))
                    {
                        // perform the action
                        this._actions[stringInstruction]();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the single directive when it is not passed as a list.
        /// </summary>
        /// <param name="actionCommand">The action command.</param>
        private void HandleSingleDirective(object actionCommand)
        {
            string instruction = actionCommand as string;

            if (instruction != null && this._actions.ContainsKey(instruction))
            {
                // perform the action
                this._actions[instruction]();
            }
        }

        /// <summary>
        /// Processes the specified arguments list.
        /// </summary>
        /// <param name="argsList">The arguments list.</param>
        /// <exception cref="System.NotImplementedException">Action processor cannot process args.</exception>
        public void Handle(IList<string> argsList)
        {
            throw new NotImplementedException();
        }
    }
}