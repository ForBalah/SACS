using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// Represents the state that the wizard is in
    /// </summary>
    public enum SetupState
    {
        /// <summary>
        /// State is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The setup is officially complete.
        /// </summary>
        Complete,

        /// <summary>
        /// The setup is closing down.
        /// </summary>
        Closing,

        /// <summary>
        /// The setup is in progress.
        /// </summary>
        InProgress
    }
}