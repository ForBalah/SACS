using System;
using System.Runtime.Serialization;
using System.Security;
using NCrontab;
using SACS.DataAccessLayer.Providers;
using Enums = SACS.Common.Enums;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// Model containing the service app details. In hindsight this should have been in a separate models project.
    /// </summary>
    public class ServiceApp
    {
        private CrontabSchedule _cronTabSchedule;

        #region Properties

        /// <summary>
        /// Gets the ServiceAppViewModel comparer
        /// </summary>
        public static Comparison<ServiceApp> Comparer
        {
            get
            {
                return new Comparison<ServiceApp>((a, b) =>
                {
                    return a.Name.CompareTo(b.Name);
                });
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the assembly file location.
        /// </summary>
        /// <value>
        /// The assembly path.
        /// </value>
        public string AppFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the startup.
        /// </summary>
        /// <value>
        /// The type of the startup.
        /// </value>
        public int StartupType { get; set; }

        /// <summary>
        /// Gets or sets the type of the startup.
        /// </summary>
        /// <value>
        /// The type of the startup.
        /// </value>
        public Enums.StartupType StartupTypeEnum
        {
            get
            {
                return (Enums.StartupType)this.StartupType;
            }

            set
            {
                this.StartupType = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        public string Schedule
        {
            get
            {
                return this._cronTabSchedule != null ? this._cronTabSchedule.ToString() : null;
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this._cronTabSchedule = CrontabSchedule.Parse(value);
                }
                else
                {
                    this._cronTabSchedule = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the last message.
        /// </summary>
        /// <value>
        /// The last message.
        /// </value>
        public string LastMessage { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state enum.
        /// </summary>
        /// <value>
        /// The state enum.
        /// </value>
        public Enums.ServiceAppState CurrentState
        {
            get
            {
                return (Enums.ServiceAppState)this.State;
            }

            set
            {
                this.State = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notifications should be sent when
        /// the service app successfully completes execution.
        /// </summary>
        public bool SendSuccessNotification { get; set; }

        /// <summary>
        /// Gets a value indicating whether the service app can be started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service app can be started.; otherwise, <c>false</c>.
        /// </value>
        public bool CanStart
        {
            get
            {
                return this.CurrentState == Enums.ServiceAppState.NotLoaded &&
                    this.StartupTypeEnum != Enums.StartupType.Disabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the service app can be stopped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service app can be stopped.; otherwise, <c>false</c>.
        /// </value>
        public bool CanStop
        {
            get
            {
                return this.CurrentState != Enums.ServiceAppState.NotLoaded;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the service app can run.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service app is running; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Even during an in-progress execution, it should be possible to run the service.
        /// Whether this is fulfilled will depend on the the actual service itself and the
        /// execution mode which it has built into it.
        /// </remarks>
        public bool CanRun
        {
            get
            {
                return this.CurrentState != Enums.ServiceAppState.NotLoaded &&
                    this.CurrentState != Enums.ServiceAppState.Unknown;
            }
        }

        /// <summary>
        /// Gets or sets the App version that this service app represents
        /// </summary>
        public Version AppVersion { get; set; }

        /// <summary>
        /// Gets or sets the SACS implementation version
        /// </summary>
        public Version SacsVersion { get; set; }

        /// <summary>
        /// Gets the image path.
        /// </summary>
        /// <value>
        /// The image path.
        /// </value>
        public string ImagePath
        {
            get
            {
                if (ImagePathProvider.Current != null)
                {
                    return ImagePathProvider.Current.GetStateImagePath(this);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ServiceApp comp = obj as ServiceApp;
            if (comp == null)
            {
                return false;
            }

            return (this.Name ?? string.Empty).Equals(comp.Name, StringComparison.OrdinalIgnoreCase) &&
                (this.Description ?? string.Empty).Equals(comp.Description, StringComparison.OrdinalIgnoreCase) &&
                (this.Environment ?? string.Empty).Equals(comp.Environment, StringComparison.OrdinalIgnoreCase) &&
                (this.AppFilePath ?? string.Empty).Equals(comp.AppFilePath, StringComparison.OrdinalIgnoreCase) &&
                this.SendSuccessNotification == comp.SendSuccessNotification &&
                this.StartupType == comp.StartupType &&
                (this.Schedule ?? string.Empty).Equals(comp.Schedule) &&
                (this.Username ?? string.Empty).Equals(comp.Username, StringComparison.OrdinalIgnoreCase) &&
                (this.Password ?? string.Empty).Equals(comp.Password, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (this.Name ?? string.Empty).GetHashCode() ^
                (this.Description ?? string.Empty).GetHashCode() ^
                (this.Environment ?? string.Empty).GetHashCode() ^
                (this.AppFilePath ?? string.Empty).GetHashCode() ^
                this.StartupType.GetHashCode() ^
                (this.Schedule ?? string.Empty).GetHashCode() ^
                (this.Username ?? string.Empty).GetHashCode() ^
                (this.Password ?? string.Empty).GetHashCode();
        }

        #endregion Methods
    }
}