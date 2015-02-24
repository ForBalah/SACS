using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// The schedule selector view
    /// </summary>
    public interface IScheduleSelectorView : IViewBase
    {
        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        string Schedule { get; }

        /// <summary>
        /// Updates the schedule selector with the provided schedule
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        void UpdateWith(string schedule);

        /// <summary>
        /// Populates the controls.
        /// </summary>
        void PopulateControls();
    }
}
