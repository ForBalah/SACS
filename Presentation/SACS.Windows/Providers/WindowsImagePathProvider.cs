using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Providers;

namespace SACS.Windows.Providers
{
    /// <summary>
    /// The Image helper class
    /// </summary>
    public class WindowsImagePathProvider : ImagePathProvider
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
                    return "/SACS.Windows;component/Images/Stop.png";
                case ServiceAppState.Initialized:
                    return serviceApp.StartupTypeEnum == StartupType.Automatic ?
                        "/SACS.Windows;component/Images/OK.png" :
                        "/SACS.Windows;component/Images/OKManual.png";
                case ServiceAppState.Executing:
                    return "/SACS.Windows;component/Images/Play.png";
                case ServiceAppState.Error:
                    return "/SACS.Windows;component/Images/Error.png";
                case ServiceAppState.Unloading:
                    return "/SACS.Windows;component/Images/Info.png";
                default:
                    return serviceApp.StartupTypeEnum == StartupType.Disabled ?
                        "/SACS.Windows;component/Images/Disabled.png" :
                        "/SACS.Windows;component/Images/Unknown.png";
            }
        }
    }
}
