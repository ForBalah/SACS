using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.Windows.ViewModels
{
    /// <summary>
    /// The ViewModel for the AppPerformance Model for WPF
    /// </summary>
    public class AppPerformanceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppPerformanceViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        public AppPerformanceViewModel(string name, IList<AppPerformance> data)
        {
            this.Name = name;
            this.Data = data ?? new List<AppPerformance>();
            this.IsSelected = true;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IList<AppPerformance> Data { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this item is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected { get; set; }
    }
}
