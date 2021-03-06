﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SACS.Common.Enums;

namespace SACS.BusinessLayer.BusinessLogic.Validation
{
    /// <summary>
    /// The service app validation class
    /// </summary>
    public class ServiceAppValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppValidator"/> class.
        /// </summary>
        public ServiceAppValidator()
        {
            this.ErrorMessages = new List<string>();
        }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        public IList<string> ErrorMessages { get; private set; }

        /// <summary>
        /// Validates the name of the application.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ValidateAppName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                this.ErrorMessages.Add("Service App name cannot be empty");
                return false;
            }
            else if (!new Regex(@"^[^\s]+[\w!@#\$%&*\(\)\[\]{}~+\- ]+$").IsMatch(name))
            {
                this.ErrorMessages.Add("Service App name is not valid. Use letters, numbers, brackets, and these symbols: !,@,#,$,&,-,+,~");
                return false;
            }
            else if (name.Contains("  "))
            {
                this.ErrorMessages.Add("Service App name is not valid. Too many spaces.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the startup type
        /// </summary>
        /// <param name="startupType">The startup type.</param>
        /// <returns></returns>
        public bool ValidateStartupType(StartupType startupType)
        {
            if (startupType == StartupType.NotSet)
            {
                this.ErrorMessages.Add("Startup type must be set");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the name of the environment.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ValidateEnvironmentName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                this.ErrorMessages.Add("Environment name cannot be empty");
                return false;
            }
            else if (!new Regex(@"^[\d\w!@#\$%&*\(\)\[\]{}~+-]+$").IsMatch(name))
            {
                this.ErrorMessages.Add("Environment name is not valid. Use letters, numbers, brackets, and these symbols: !,@,#,$,&,-,+,~");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public bool ValidateDescription(string description)
        {
            // The description can be null or empty or anything
            return true;
        }

        /// <summary>
        /// Validates the app file path.
        /// </summary>
        /// <param name="name">The name of the app file to run.</param>
        /// <returns></returns>
        public bool ValidateAppFilePath(string name)
        {
            return this.ValidatePath(name, "App file path");
        }

        /// <summary>
        /// Validates the schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns></returns>
        public bool ValidateSchedule(string schedule)
        {
            NCrontab.CrontabSchedule cronSchedule = NCrontab.CrontabSchedule.TryParse(schedule);
            if (cronSchedule == null)
            {
                this.ErrorMessages.Add("Schedule is not valid");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the entire service app.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        /// <returns></returns>
        public bool ValidateServiceApp(DataAccessLayer.Models.ServiceApp serviceApp)
        {
            if (serviceApp == null)
            {
                return false;
            }

            bool isValid = true;
            isValid &= this.ValidateAppName(serviceApp.Name);
            isValid &= this.ValidateStartupType(serviceApp.StartupTypeEnum);
            isValid &= this.ValidateEnvironmentName(serviceApp.Environment);
            isValid &= this.ValidateDescription(serviceApp.Description);
            isValid &= this.ValidateAppFilePath(serviceApp.AppFilePath);
            isValid &= this.ValidateSchedule(serviceApp.Schedule);

            return isValid;
        }

        /// <summary>
        /// Clears the errors
        /// </summary>
        public void ClearErrors()
        {
            this.ErrorMessages.Clear();
        }

        /// <summary>
        /// Validates the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool ValidatePath(string path, string type)
        {
            // The path must always be the full path for security reasons
            if (string.IsNullOrWhiteSpace(path))
            {
                this.ErrorMessages.Add(type + " cannot be empty");
                return false;
            }
            else if (!Path.IsPathRooted(path))
            {
                this.ErrorMessages.Add(type + " must be a full rooted path");
                return false;
            }
            else
            {
                try
                {
                    Path.GetFullPath(path);
                }
                catch
                {
                    this.ErrorMessages.Add(type + " is not valid");
                    return false;
                }
            }

            return true;
        }
    }
}