using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// Base interface for views
    /// </summary>
    public interface IViewBase
    {
        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        void ShowException(string title, Exception e);
    }
}
