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
using System.Collections;
using AIStudio.Wpf.ADiagram.Helpers;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Controls
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeList;
        public MultiSelectComboBox()
        {
            InitializeComponent();
            _nodeList = new ObservableCollection<Node>();
        }

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register("ItemsSource", typeof(IList), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
         DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
     new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty SelectedValuesProperty =
       DependencyProperty.Register("SelectedValues", typeof(IList), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
   new PropertyChangedCallback(MultiSelectComboBox.OnSelectedValuesChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public string DisplayMemberPath { get; set; }

        public string SelectedValuePath { get; set; }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public IList SelectedValues
        {
            get { return (IList)GetValue(SelectedValuesProperty); }
            set
            {
                SetValue(SelectedValuesProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        #endregion

        #region Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private static void OnSelectedValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if (clickedBox.Content.ToString() == "All")
            {
                if (clickedBox.IsChecked.Value)
                {
                    foreach (Node node in _nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
                else
                {
                    foreach (Node node in _nodeList)
                    {
                        node.IsSelected = false;
                    }
                }

            }
            else
            {
                int _selectedCount = 0;
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Object.ToString() != "All")
                        _selectedCount++;
                }
                if (_selectedCount == _nodeList.Count - 1)
                    _nodeList.FirstOrDefault(i => i.Object.ToString() == "All").IsSelected = true;
                else
                    _nodeList.FirstOrDefault(i => i.Object.ToString() == "All").IsSelected = false;
            }
            SetSelectedItems();
            SetText();

        }
        #endregion


        #region Methods
        private void SelectNodes()
        {
            if (SelectedItems != null)
            {
                foreach (var item in SelectedItems)
                {
                    Node node = _nodeList.FirstOrDefault(i => i.Object == item);
                    if (node != null)
                        node.IsSelected = true;
                }
            }
            else if (SelectedValues != null)
            {
                foreach (var item in SelectedValues)
                {
                    Node node = _nodeList.FirstOrDefault(i => i.Object != null && i.Object.ToString() != "All" && i.Object.GetPropertyValue(SelectedValuePath) == item);
                    if (node != null)
                        node.IsSelected = true;
                }
            }
        }

        private void SetSelectedItems()
        {
            if (SelectedItems != null)
            {
                SelectedItems.Clear();
                foreach (Node node in _nodeList)
                {
                    if (node.IsSelected && node.Object.ToString() != "All")
                    {
                        if (this.ItemsSource.Count > 0)
                        {
                            if (SelectedItems != null)
                            {
                                SelectedItems.Add(node.Object);
                            }
                        }
                    }
                }
            }

            if (SelectedValues != null)
            {
                SelectedValues.Clear();
                foreach (Node node in _nodeList)
                {
                    if (node.IsSelected && node.Object.ToString() != "All")
                    {
                        if (this.ItemsSource.Count > 0)
                        {
                            if (SelectedValues != null)
                            {
                                SelectedValues.Add(node.Object.GetPropertyValue(SelectedValuePath));
                            }
                        }
                    }
                }
            }
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (this.ItemsSource.Count > 0)
                _nodeList.Add(new Node("All", DisplayMemberPath));
            foreach (var item in this.ItemsSource)
            {
                Node node = new Node(item, DisplayMemberPath);
                _nodeList.Add(node);
            }
            MultiSelectCombo.ItemsSource = _nodeList;
        }

        private void SetText()
        {
            StringBuilder displayText = new StringBuilder();
            foreach (Node s in _nodeList)
            {
                if (s.IsSelected == true && s.Object.ToString() == "All")
                {
                    displayText = new StringBuilder();
                    displayText.Append("All");
                    break;
                }
                else if (s.IsSelected == true && s.Object.ToString() != "All")
                {
                    displayText.Append(s.Object);
                    displayText.Append(',');
                }
            }
            this.Text = displayText.ToString().TrimEnd(new char[] { ',' });

            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }


        #endregion
    }

    public class Node : INotifyPropertyChanged
    {
        #region ctor
        public Node(object obj, string displayMemberPath)
        {
            Object = obj;

            if (!string.IsNullOrEmpty(displayMemberPath) && Object.ContainsProperty(displayMemberPath))
                Title = Object.GetPropertyValue(displayMemberPath).ToString();
            else
                Title = obj.ToString();
        }
        #endregion

        #region Properties
        private object _object;
        public object Object
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
                NotifyPropertyChanged("Object");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
