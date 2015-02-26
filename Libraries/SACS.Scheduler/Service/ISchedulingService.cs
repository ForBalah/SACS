using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Scheduler.Service
{
    /// <summary>
    /// The Scheduling service
    /// </summary>
    public interface ISchedulingService
    {
        /// <summary>
        /// Gets a value indicating whether this schedule service is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Add the named job to the service given the execution step to perform with the specified schedule.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="schedule">The crontab schedule string.</param>
        /// <param name="executionStep">The step to perform on execution.</param>
        void AddJob(string name, string schedule, Action executionStep);

        /// <summary>
        /// Add the named job to the service given the execution step to perform with the specified schedule.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="schedule">The schedule defining function.</param>
        /// <param name="executionStep">The step to perform on execution.</param>
        void AddJob(string name, Func<DateTime, DateTime> schedule, Action executionStep);

        /// <summary>
        /// Returns the name job.
        /// </summary>
        /// <param name="name">The job name.</param>
        /// <returns></returns>
        IServiceJob GetJob(string name);

        /// <summary>
        /// Returns true if this service already has a job of that name loaded into it.
        /// </summary>
        /// <param name="name">The job name.</param>
        /// <returns></returns>
        bool HasJob(string name);

        /// <summary>
        /// Starts the scheduling service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the scheduling service and all jobs contained within.
        /// </summary>
        void Stop();

        /// <summary>
        /// Immediately runs a job specified by name.
        /// </summary>
        /// <param name="name">The job name.</param>
        void RunJob(string name);

        /// <summary>
        /// Stops and removes named job from this service.
        /// </summary>
        /// <param name="name">The job name.</param>
        void RemoveJob(string name);
    }
}
