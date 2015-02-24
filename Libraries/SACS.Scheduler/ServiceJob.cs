using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SACS.Scheduler
{
    /// <summary>
    /// The service job class.
    /// </summary>
    public class ServiceJob : IServiceJob
    {
        ILog _log = LogManager.GetLogger(typeof(ServiceJob));
        private Action _executionStep;

        public ServiceJob(string name, Func<DateTime, DateTime> schedule, Action executionStep)
        {
            this.Name = name;
            this.NextOccurence = schedule;
            this._executionStep = executionStep;
        }

        /// <summary>
        /// Gets the name of this service job
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the next occurence function for this job
        /// </summary>
        public Func<DateTime, DateTime> NextOccurence { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this job is currently executing. Note this is only reliable if the execution step is synchronous,
        /// otherwise this will most likely always be false.
        /// </summary>
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// Executes this ServiceJob instance
        /// </summary>
        public void Execute()
        {
            this.IsExecuting = true;
            try
            {
                this._executionStep();
            }
            catch (Exception e)
            {
                this._log.Warn(string.Format("The service Job {0} did not execute sucessfully", this.Name), e);
            }
            finally
            {
                this.IsExecuting = false;
            }
        }
    }
}
