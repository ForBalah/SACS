using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SACS.Windows.ExtentedControls
{
    /// <summary>
    /// Fixed Width Column
    /// </summary>
    public class FixedWidthColumn : GridViewColumn
    {
        /// <summary>
        /// The fixed width property
        /// </summary>
        public static readonly DependencyProperty FixedWidthProperty =
            DependencyProperty.Register(
                "FixedWidth",
                typeof(double),
                typeof(FixedWidthColumn),
                new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnFixedWidthChanged)));

        /// <summary>
        /// Initializes the <see cref="FixedWidthColumn"/> class.
        /// </summary>
        static FixedWidthColumn()
        {
            WidthProperty.OverrideMetadata(
                typeof(FixedWidthColumn), 
                new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceWidth)));
        }

        /// <summary>
        /// Gets or sets the width of the fixed.
        /// </summary>
        /// <value>
        /// The width of the fixed.
        /// </value>
        public double FixedWidth
        {
            get
            {
                return (double)this.GetValue(FixedWidthProperty);
            }

            set
            {
                this.SetValue(FixedWidthProperty, value);
            }
        }

        /// <summary>
        /// Called when the fixed width changed.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFixedWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            FixedWidthColumn fwc = o as FixedWidthColumn;
            if (fwc != null)
            {
                fwc.CoerceValue(WidthProperty);
            }
        }

        /// <summary>
        /// Called when width is coerced... I guess.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object OnCoerceWidth(DependencyObject o, object baseValue)
        {
            FixedWidthColumn fwc = o as FixedWidthColumn;
            if (fwc != null)
            {
                return fwc.FixedWidth;
            }

            return baseValue;
        }
    }
}
