using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.BusinessLogic.Schedule;
using SACS.Common.Enums;
using Models = SACS.DataAccessLayer.Models;

namespace SACS.Windows.ViewModels
{
    /// <summary>
    /// The view model which is an extension of the ServiceApp
    /// </summary>
    public class ServiceAppViewModel
    {
        /// <summary>
        /// The ServiceAppViewModel comparer
        /// </summary>
        public static Comparison<ServiceAppViewModel> Comparer = new Comparison<ServiceAppViewModel>((a, b) =>
        {
            return a.ServiceApp.Name.CompareTo(b.ServiceApp.Name);
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppViewModel"/> class.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        public ServiceAppViewModel(Models.ServiceApp serviceApp)
        {
            if (serviceApp != null)
            {
                this.ServiceApp = serviceApp;
            }
            else
            {
                this.ServiceApp = new Models.ServiceApp
                {
                    StartupTypeEnum = StartupType.NotSet,
                    Schedule = string.Empty
                };
            }
        }

        /// <summary>
        /// Gets the service application.
        /// </summary>
        /// <value>
        /// The service application.
        /// </value>
        public Models.ServiceApp ServiceApp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the image path used to display the state of the service
        /// </summary>
        /// <value>
        /// The image path.
        /// </value>
        public string ImagePath
        {
            get
            {
                switch (this.ServiceApp.StateEnum)
                {
                    case ServiceAppState.NotLoaded:
                        return "/SACS.Windows;component/Images/Stop.png";
                    case ServiceAppState.Initialized:
                        return this.ServiceApp.StartupTypeEnum == StartupType.Automatic ? 
                            "/SACS.Windows;component/Images/OK.png" : 
                            "/SACS.Windows;component/Images/OKManual.png";
                    case ServiceAppState.Executing:
                        return "/SACS.Windows;component/Images/Play.png";
                    case ServiceAppState.Error:
                        return "/SACS.Windows;component/Images/Error.png";
                    case ServiceAppState.Unloading:
                        return "/SACS.Windows;component/Images/Info.png";
                    default:
                        return this.ServiceApp.StartupTypeEnum == StartupType.Disabled ?
                            "/SACS.Windows;component/Images/Disabled.png" :
                            "/SACS.Windows;component/Images/Unknown.png";
                }
            }
        }

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
                bool appState = this.ServiceApp.StateEnum == ServiceAppState.Unknown ||
                    this.ServiceApp.StateEnum == ServiceAppState.NotLoaded ||
                    this.ServiceApp.StateEnum == ServiceAppState.Error;
                return appState && this.ServiceApp.StartupTypeEnum != StartupType.Disabled;
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
                return this.ServiceApp.StateEnum == ServiceAppState.Initialized ||
                    this.ServiceApp.StateEnum == ServiceAppState.Executing;
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
                return this.ServiceApp.StateEnum == ServiceAppState.Initialized ||
                    this.ServiceApp.StateEnum == ServiceAppState.Executing;
            }
        }

        /// <summary>
        /// Gets the schedule description.
        /// </summary>
        /// <value>
        /// The schedule description.
        /// </value>
        public string ScheduleDescription
        {
            get
            {
                return ScheduleUtility.GetFullDescription(this.ServiceApp.Schedule);
            }
        }
    }
}
