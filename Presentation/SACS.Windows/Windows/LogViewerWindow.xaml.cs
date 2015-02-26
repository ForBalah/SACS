using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SACS.Windows.Windows
{
    /// <summary>
    /// Interaction logic for LogsWindow.xaml
    /// </summary>
    public partial class LogViewerWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewerWindow"/> class.
        /// </summary>
        public LogViewerWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SetFileName(string fileName)
        {
            this.LogDetail.SetFileName(fileName);
        }
    }
}
