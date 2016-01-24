using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// The navigation validator
    /// </summary>
    public interface INavigationValidator
    {
        /// <summary>
        /// Validates the ability to go next.
        /// </summary>
        /// <returns></returns>
        bool ValidateGoNext();
    }
}