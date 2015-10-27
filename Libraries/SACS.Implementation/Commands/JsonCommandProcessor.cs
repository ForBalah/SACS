using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SACS.Implementation.Commands
{
    /// <summary>
    /// The JSON version of the command line processor.
    /// </summary>
    internal class JsonCommandProcessor : CommandLineProcessor
    {
        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        internal override void Process(string command)
        {
        }

        /// <summary>
        /// Parses the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal override IDictionary<string, object> Parse(string command)
        {
            var commandTree = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(command))
            {
                commandTree.Add(string.Empty, null);
                return commandTree;
            }

            RecursiveTryParse(string.Empty, command.Replace("{", " {"), commandTree);

            try
            {
                // only if this isn't json will 'exe' work
                commandTree.Add("exe", Path.GetFullPath(command));
            }
            catch
            {
                // do nothing
            }

            return commandTree;
        }

        /// <summary>
        /// Processes the command recursively, trying to parse each part of it into a dictionary
        /// from JSON.
        /// </summary>
        /// <param name="leftCommand">The left command to process independently.</param>
        /// <param name="rightCommand">The right command to process independently.</param>
        /// <param name="commandTree">The object to put the results into.</param>
        private static void RecursiveTryParse(string leftCommand, string rightCommand, Dictionary<string, object> commandTree)
        {
            // do the right-hand side first
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                string cleanedCommand = rightCommand.Trim();
                if (!cleanedCommand.StartsWith("{") || !cleanedCommand.EndsWith("}"))
                {
                    cleanedCommand = string.Format("{{ {0} }}", cleanedCommand);
                }

                var result = serializer.DeserializeObject(cleanedCommand);
                commandTree.Add("commands", result);

                // TODO: process the left-hand side
            }
            catch (ArgumentException)
            {
                // split the right-hand side, add it to the left-hand side and try again
                var splitCommand = rightCommand.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitCommand.Length > 0)
                {
                    RecursiveTryParse(string.Format("{0} {1}", leftCommand, splitCommand[0]), string.Join(" ", splitCommand.Skip(1)), commandTree);
                }
            }
        }
    }
}
