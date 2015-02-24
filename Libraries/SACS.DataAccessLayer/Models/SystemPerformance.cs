using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// the system performance model
    /// </summary>
    public class SystemPerformance
    {
        /// <summary>
        /// Gets or sets the audit time.
        /// </summary>
        /// <value>
        /// The audit time.
        /// </value>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Value { get; set; }
    }
}
