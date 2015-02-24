using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SACS.Windows.Extensions;

namespace MultiSelectComboBox
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml. Note this is a bit of a hacked together control so if a
    /// better multi select implemetation for WPF is out there, feel free to swap it out for that. E.g This version
    /// has not been made generic
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectComboBox"/> class.
        /// </summary>
        public MultiSelectComboBox()
        {
            InitializeComponent();
            this._nodeList = new ObservableCollection<Node>();
        }

        #region Events
        
        /// <summary>
        /// Occurs when the drop down list in the combo closes
        /// </summary>
        public event EventHandler DropDownClosed; 

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public Dictionary<string, object> ItemsSource
        {
            get { return (Dictionary<string, object>)GetValue(ItemsSourceProperty); }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public Dictionary<string, object> SelectedItems
        {
            get { return (Dictionary<string, object>)GetValue(SelectedItemsProperty); }

            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default text.
        /// </summary>
        /// <value>
        /// The default text.
        /// </value>
        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }

            set { SetValue(DefaultTextProperty, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether all items checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all items checked; otherwise, <c>false</c>.
        /// </value>
        public bool AllItemsChecked
        {
            get;
            private set;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when [items source changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        /// <summary>
        /// Called when [selected items changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        /// <summary>
        /// Handles the DropDownClosed event of the MultiSelectCombo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MultiSelectCombo_DropDownClosed(object sender, EventArgs e)
        {
            if (this.DropDownClosed != null)
            {
                this.DropDownClosed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if ((string)clickedBox.Content == "All")
            {
                if (clickedBox.IsChecked.Value)
                {
                    foreach (Node node in this._nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
                else
                {
                    foreach (Node node in this._nodeList)
                    {
                        node.IsSelected = false;
                    }
                }
            }
            else
            {
                int selectedCount = 0;
                foreach (Node s in this._nodeList)
                {
                    if (s.IsSelected && s.Title != "All")
                    {
                        selectedCount++;
                    }
                }
                if (selectedCount == this._nodeList.Count - 1)
                {
                    this._nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = true;
                }
                else
                {
                    this._nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = false;
                }
            }

            this.SetSelectedItems();
            this.SetText();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all selected items
        /// </summary>
        public void ClearAll()
        {
            foreach (Node node in this._nodeList)
            {
                node.IsSelected = false;
            }

            this.SetSelectedItems();
            this.SetText();
        }

        /// <summary>
        /// Selects all items
        /// </summary>
        public void SelectAll()
        {
            if (this.MultiSelectCombo.HasItems)
            {
                foreach (Node node in this._nodeList)
                {
                    node.IsSelected = true;
                }
            }

            this.SetSelectedItems();
            this.SetText();
        }

        /// <summary>
        /// Sets the select values. This could potentially be really really slow (O(n^3)) so do not use
        /// this for medium-large lists without optimizing this
        /// </summary>
        /// <param name="values">The values.</param>
        public void SetSelectedValues(IList<object> values)
        {
            if (values == null)
            {
                this.ClearAll();
            }
            else
            {
                var itemsToSelect = this.ItemsSource
                    .Where(i => values.Contains(i.Value))
                    .Select(i => i.Key)
                    .ToList();

                foreach (Node node in this._nodeList)
                {
                    node.IsSelected = itemsToSelect.Contains(node.Title);
                }
            }

            this.SetSelectedItems();
            this.SetText();
        }

        /// <summary>
        /// Sets the selected value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetSelectedValue(object value)
        {
            if (value == null)
            {
                this.ClearAll();
            }
            else
            {
                var itemsToSelect = this.ItemsSource
                    .Where(i => i.Value == value)
                    .Select(i => i.Key)
                    .ToList();

                foreach (Node node in this._nodeList)
                {
                    node.IsSelected = itemsToSelect.Contains(node.Title);
                }
            }

            this.SetSelectedItems();
            this.SetText();
        }

        /// <summary>
        /// Selects the nodes.
        /// </summary>
        private void SelectNodes()
        {
            foreach (KeyValuePair<string, object> keyValue in this.SelectedItems)
            {
                Node node = this._nodeList.FirstOrDefault(i => i.Title == keyValue.Key);
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        private void SetSelectedItems()
        {
            if (this.SelectedItems == null)
            {
                this.SelectedItems = new Dictionary<string, object>();
            }

            this.SelectedItems.Clear();
            foreach (Node node in this._nodeList)
            {
                if (node.IsSelected && node.Title != "All")
                {
                    if (this.ItemsSource.Count > 0)
                    {
                        this.SelectedItems.Add(node.Title, this.ItemsSource[node.Title]);
                    }
                }

                if (node.Title == "All")
                {
                    this.AllItemsChecked = node.IsSelected;
                }
            }
        }

        /// <summary>
        /// Displays the in control.
        /// </summary>
        private void DisplayInControl()
        {
            this._nodeList.Clear();
            if (this.ItemsSource.Count > 0)
            {
                this._nodeList.Add(new Node("All"));
            }

            foreach (KeyValuePair<string, object> keyValue in this.ItemsSource)
            {
                Node node = new Node(keyValue.Key);
                this._nodeList.Add(node);
            }

            this.MultiSelectCombo.ItemsSource = this._nodeList;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (Node s in this._nodeList)
                {
                    if (s.IsSelected == true && s.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }
                    else if (s.IsSelected == true && s.Title != "All")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(',');
                    }
                }

                this.Text = displayText.ToString().TrimEnd(new char[] { ',' });
            }

            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }

        #endregion
    }

    /// <summary>
    /// The node class for the multiselect
    /// </summary>
    public class Node : INotifyPropertyChanged
    {
        private string _title;
        private bool _isSelected;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public Node(string title)
        {
            this.Title = title;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return this._title;
            }

            set
            {
                this._title = value;
                this.NotifyPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is selected]; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }

            set
            {
                this._isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        #endregion

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
