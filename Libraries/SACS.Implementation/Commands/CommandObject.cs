using System.Collections.Generic;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// Represents a collection of commands.
    /// </summary>
    public class CommandObject : Dictionary<string, object>
    {
        /// <summary>
        /// Gets the commands dictionary.
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetCommands()
        {
            if (this.ContainsKey("commands"))
            {
                return this["commands"] as Dictionary<string, object>;
            }

            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the args list.
        /// </summary>
        /// <returns></returns>
        public virtual IList<string> GetArgs()
        {
            if (this.ContainsKey("args"))
            {
                return this["args"] as List<string>;
            }

            return new List<string>();
        }
    }
}