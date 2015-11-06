using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Commands
{
    internal class DirectiveProcessor : ICommandProcessor
    {
        private Dictionary<string, Action> _directives = new Dictionary<string, Action>();

        public string Command
        {
            get
            {
                return "miscelaneous directives";
            }
        }

        public CommandProcessorType Type
        {
            get
            {
                return CommandProcessorType.Command;
            }
        }

        public ICommandProcessor For(string argument, Action action)
        {
            this._directives[argument] = action;
            return this;
        }

        public void Process(IDictionary<string, object> commandObject)
        {
            //object instruction;

            //foreach (var directive in this._directives.Keys)
            //{
            //    if (commandObject.TryGetValue(directive, out instruction))
            //    {
            //        this._directives[directive]()
            //    }
            //}

            //if (commandObject.TryGetValue(this.Command, out directive))
            //{
            //    string instruction = actionCommand as string;

            //    if (this._actions.ContainsKey(instruction))
            //    {
            //        // perform the action
            //        this._actions[instruction]();
            //    }

            //    // we are done processing it
            //    commandObject.Remove(this.Command);
            //}
        }

        public void Process(IList<string> argsList)
        {
            throw new NotImplementedException();
        }
    }
}