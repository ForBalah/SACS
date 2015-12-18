using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NCrontab;

namespace SACS.Scheduler.Service
{
    /// <summary>
    /// The Scheduling service class
    /// </summary>
    public class SchedulingService : ISchedulingService
    {
        private readonly Dictionary<IServiceJob, Timer> _jobs = new Dictionary<IServiceJob, Timer>();
        private readonly Timer _timerMonitor = new Timer(30000);
        private readonly List<Timer> _timers = new List<Timer>();

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
            // clean up timers
            for (int i = this._timers.Count - 1; i >= 0; i--)
            {
                var timer = this._timers[i];
                if (!timer.Enabled)
                {
                    this._timers.RemoveAt(i);
                    timer.Dispose();
                }
            }
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
            this.IsRunning = false;
            foreach (var job in this._jobs)
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
            var job = this.GetJob(name);
            job.Execute();
        }

        /// <summary>
        /// Stops and removes named job from this service.
        /// </summary>
        /// <param name="name">The job name.</param>
        public void RemoveJob(string name)
        {
            if (this.HasJob(name))
            {
                var entry = this._jobs.FirstOrDefault(i => JobComparison(i.Key, name));
                entry.Value.Stop();
                this._jobs.Remove(entry.Key);
            }
        }

        /// <summary>
        /// Schedules the next execution of the specified job
        /// </summary>
        /// <param name="job">The job.</param>
        internal void ScheduleNextExecution(IServiceJob job)
        {
            var currentTime = SystemTime.Now;
            var nextTime = job.NextOccurence(currentTime);
            Timer timer = new Timer((nextTime - currentTime).TotalMilliseconds);
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
    }
}