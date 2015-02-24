using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// Represents the result of the attempt to build a crontab schedule
    /// </summary>
    public class ScheduleBuildResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleBuildResult"/> class.
        /// </summary>
        public ScheduleBuildResult()
        {
            this.Messages = new List<string>();
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public string Result { get; internal set; }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IList<string> Messages { get; private set; }
    }
}
