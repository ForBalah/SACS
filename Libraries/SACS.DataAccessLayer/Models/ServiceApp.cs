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
        /// Gets or sets the base path of the service app
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name of assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets the assembly file location.
        /// </summary>
        /// <value>
        /// The assembly path.
        /// </value>
        public string AssemblyPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Path) || string.IsNullOrWhiteSpace(this.EntryFile))
                {
                    return string.Empty;
                }

                return System.IO.Path.Combine(this.Path, this.EntryFile);
            }
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
        /// Gets or sets the configuration file path.
        /// </summary>
        /// <value>
        /// The configuration file path.
        /// </value>
        public string ConfigFilePath { get; set; }

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
        public Enums.ServiceAppState StateEnum
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
        /// Gets or sets the entry file.
        /// </summary>
        /// <value>
        /// The entry file.
        /// </value>
        public string EntryFile { get; set; }

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
        /// Gets a value indicating whether the service app can be started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service app can be started.; otherwise, <c>false</c>.
        /// </value>
        public bool CanStart
        {
            get
            {
                bool appState = this.StateEnum == Enums.ServiceAppState.Unknown ||
                    this.StateEnum == Enums.ServiceAppState.NotLoaded ||
                    this.StateEnum == Enums.ServiceAppState.Error;
                return appState && this.StartupTypeEnum != Enums.StartupType.Disabled;
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
                return this.StateEnum == Enums.ServiceAppState.Initialized ||
                    this.StateEnum == Enums.ServiceAppState.Executing;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the service app is running
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service app is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return this.StateEnum == Enums.ServiceAppState.Initialized ||
                    this.StateEnum == Enums.ServiceAppState.Executing;
            }
        }

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
                (this.Path ?? string.Empty).Equals(comp.Path, StringComparison.OrdinalIgnoreCase) &&
                (this.Assembly ?? string.Empty).Equals(comp.Assembly, StringComparison.OrdinalIgnoreCase) &&
                this.StartupType == comp.StartupType &&
                (this.Schedule ?? string.Empty).Equals(comp.Schedule) &&
                (this.ConfigFilePath ?? string.Empty).Equals(comp.ConfigFilePath, StringComparison.OrdinalIgnoreCase) &&
                (this.EntryFile ?? string.Empty).Equals(comp.EntryFile, StringComparison.OrdinalIgnoreCase) &&
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
                (this.Path ?? string.Empty).GetHashCode() ^
                (this.Assembly ?? string.Empty).GetHashCode() ^
                this.StartupType.GetHashCode() ^
                (this.Schedule ?? string.Empty).GetHashCode() ^
                (this.ConfigFilePath ?? string.Empty).GetHashCode() ^
                (this.EntryFile ?? string.Empty).GetHashCode() ^
                (this.Username ?? string.Empty).GetHashCode() ^
                (this.Password ?? string.Empty).GetHashCode();
        }
    }
}
