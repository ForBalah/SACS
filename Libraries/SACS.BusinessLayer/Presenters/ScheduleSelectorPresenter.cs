using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.Views;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// The schedule selector presenter
    /// </summary>
    public class ScheduleSelectorPresenter : PresenterBase<IScheduleSelectorView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleSelectorPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ScheduleSelectorPresenter(IScheduleSelectorView view)
            : base(view)
        {
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        public void LoadControl()
        {
            this.View.PopulateControls();
        }
    }
}
