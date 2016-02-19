using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using NCrontab;

namespace SACS.Scheduler.Service
{
    /// <summary>
    /// The Scheduling service class
    /// </summary>
    public class SchedulingService : ISchedulingService
    {
        private readonly Dictionary<IServiceJob, PersistentTimer> _jobs = new Dictionary<IServiceJob, PersistentTimer>();
        private readonly PersistentTimer _timerMonitor = new PersistentTimer(30000, "TimerMonitor");
        private readonly List<PersistentTimer> _timers = new List<PersistentTimer>();
        private ILog _log = LogManager.GetLogger(typeof(SchedulingService));

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingService"/> class.
        /// </summary>
        public SchedulingService()
        {
            this._timerMonitor.Elapsed += this.TimerMonitor_Elapsed;
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this schedule service is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Elapsed event of the TimerMonitor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void TimerMonitor_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<string> cleanedTimers = new List<string>();

            // clean up timers
            for (int i = this._timers.Count - 1; i >= 0; i--)
            {
                var timer = this._timers[i];
                if (!timer.Enabled)
                {
                    cleanedTimers.Add(timer.Name);
                    this._timers.RemoveAt(i);
                    timer.Dispose();
                }
            }

            this._log.Debug(string.Format("SchedulingService.TimerMonitor_Elapsed(,). Cleaned up: {0}", string.Join(",", cleanedTimers)));
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Add the named job to the service given the execution step to perform with the specified schedule.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="schedule">The crontab schedule string.</param>
        /// <param name="executionStep">The step to perform on execution.</param>
        public void AddJob(string name, string schedule, Action executionStep)
        {
            var cronSchedule = CrontabSchedule.Parse(schedule);
            this.AddJob(name, cronSchedule.GetNextOccurrence, executionStep);
        }

        /// <summary>
        /// Add the named job to the service given the execution step to perform with the specified schedule.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="schedule">The schedule defining function.</param>
        /// <param name="executionStep">The step to perform on execution.</param>
        public void AddJob(string name, Func<DateTime, DateTime> schedule, Action executionStep)
        {
            this._log.Debug(string.Format("SchedulingService.AddJob({0})", name));
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "Job Name cannot be null");
            }

            if (schedule == null)
            {
                throw new ArgumentNullException("schedule", "The scheduling function cannot be null");
            }

            if (executionStep == null)
            {
                throw new ArgumentNullException("executionStep", "The exectution step cannot be null");
            }

            if (this._jobs.Any(i => JobComparison(i.Key, name)))
            {
                throw new ArgumentException("An job with the same name already exists in SchedulingService");
            }

            IServiceJob job = new ServiceJob(name, schedule, executionStep);
            this._jobs.Add(job, null);
            this.ScheduleNextExecution(job);
        }

        /// <summary>
        /// Returns true if this service already has a job of that name loaded into it.
        /// </summary>
        /// <param name="name">The job name</param>
        /// <returns></returns>
        public bool HasJob(string name)
        {
            return this._jobs.Any(i => JobComparison(i.Key, name));
        }

        /// <summary>
        /// Returns the name job
        /// </summary>
        /// <param name="name">The job name.</param>
        /// <returns></returns>
        public IServiceJob GetJob(string name)
        {
            if (this.HasJob(name))
            {
                return this._jobs.FirstOrDefault(i => JobComparison(i.Key, name)).Key;
            }

            return null;
        }

        /// <summary>
        /// Starts the scheduling service
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            this._timerMonitor.Start();
        }

        /// <summary>
        /// Stops the scheduling service and all jobs contained within
        /// </summary>
        public void Stop()
        {
            this._log.Debug("SchedulingService.Stop()");
            this.IsRunning = false;
            foreach (var job in this._jobs.Where(j => j.Value != null))
            {
                job.Value.Stop();
            }
        }

        /// <summary>
        /// Immediately runs a job specified by name.
        /// </summary>
        /// <param name="name">The job name.</param>
        public void RunJob(string name)
        {
            this._log.Debug(string.Format("SchedulingService.RunJob({0})", name));
            var job = this.GetJob(name);
            job.Execute();
        }

        /// <summary>
        /// Stops and removes named job from this service.
        /// </summary>
        /// <param name="name">The job name.</param>
        public void RemoveJob(string name)
        {
            this._log.Debug(string.Format("SchedulingService.RemoveJob({0})", name));
            if (this.HasJob(name))
            {
                var entry = this._jobs.FirstOrDefault(i => JobComparison(i.Key, name));

                try
                {
                    if (entry.Value != null)
                    {
                        entry.Value.Stop();
                    }
                }
                finally
                {
                    this._jobs.Remove(entry.Key);
                }
            }
        }

        /// <summary>
        /// Schedules the next execution of the specified job
        /// </summary>
        /// <param name="job">The job.</param>
        internal void ScheduleNextExecution(IServiceJob job)
        {
            this._log.Debug(string.Format("SchedulingService.ScheduleNextExecution({0})", job.Name));
            var currentTime = SystemTime.Now;
            var nextTime = job.NextOccurence(currentTime);
            PersistentTimer timer = new PersistentTimer((nextTime - currentTime).TotalMilliseconds, job.Name);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Elapsed += (source, e) =>
            {
                timer.Stop();
                if (!job.IsExecuting && this.IsRunning)
                {
                    job.Execute();
                }

                this.ScheduleNextExecution(job);
            };

            this._timers.Add(timer);

            if (this._jobs[job] != null)
            {
                this._jobs[job].Stop();
                this._jobs[job].Dispose();
            }

            this._jobs[job] = timer; // overwrite the timer.
        }

        /// <summary>
        /// Returns a boolean indicating whether the name matches the job
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="name">The name to compare to.</param>
        /// <returns></returns>
        private static bool JobComparison(IServiceJob job, string name)
        {
            return job.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion Methods

        private class PersistentTimer : Timer
        {
            /// <summary>
            /// Gets the name of the timer
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="PersistentTimer"/> class.
            /// </summary>
            /// <param name="interval">The time, in milliseconds, between events. The value
            /// must be greater than zero and less than or equal to System.Int32.MaxValue.</param>
            /// <param name="name">The name associated with the timer.</param>
            public PersistentTimer(double interval, string name)
                : base(interval)
            {
                this.Name = name;
            }

            /// <summary>
            /// Obtains a lifetime service object to control the lifetime policy for this instance.
            /// </summary>
            /// <returns>
            /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy
            /// for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a
            /// new lifetime service object initialized to the value of the
            /// <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
            /// </returns>
            public override object InitializeLifetimeService()
            {
                return null;
            }
        }
    }
}