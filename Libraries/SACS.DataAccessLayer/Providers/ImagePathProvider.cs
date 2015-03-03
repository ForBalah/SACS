using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.Providers
{
    /// <summary>
    /// The Service App image path provider
    /// </summary>
    public abstract class ImagePathProvider
    {
        /// <summary>
        /// Gets the current image path provider
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static ImagePathProvider Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void SetProvider(ImagePathProvider provider)
        {
            Current = provider;
        }

        /// <summary>
        /// Gets the image path for the state of the service app.
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        /// <returns></returns>
        public abstract string GetStateImagePath(ServiceApp serviceApp);
    }
}
