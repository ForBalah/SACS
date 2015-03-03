using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Providers;

namespace SACS.Web.PresentationLogic.Providers
{
    /// <summary>
    /// The Image helper class
    /// </summary>
    public class WebImagePathProvider : ImagePathProvider
    {
        /// <summary>
        /// Gets the image path for the state of the service app.
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        /// <returns></returns>
        public override string GetStateImagePath(DataAccessLayer.Models.ServiceApp serviceApp)
        {
            switch (serviceApp.StateEnum)
            {
                case ServiceAppState.NotLoaded:
                    return "~/Content/Images/Stop.png";
                case ServiceAppState.Initialized:
                    return serviceApp.StartupTypeEnum == StartupType.Automatic ?
                        "~/Content/Images/OK.png" :
                        "~/Content/Images/OKManual.png";
                case ServiceAppState.Executing:
                    return "~/Content/Images/Play.png";
                case ServiceAppState.Error:
                    return "~/Content/Images/Error.png";
                case ServiceAppState.Unloading:
                    return "~/Content/Images/Info.png";
                default:
                    return serviceApp.StartupTypeEnum == StartupType.Disabled ?
                        "~/Content/Images/Disabled.png" :
                        "~/Content/Images/Unknown.png";
            }
        }
    }
}