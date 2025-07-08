using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TuDus.Models;
using TuDus.Parser;

namespace TuDus.CustomElements
{
    /// <summary>
    /// Interaction logic for TuDuObject.xaml
    /// </summary>
    public partial class TuDuObject : UserControl
    {
        private TuDuItem _tuDuItem;

        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.Register("DescriptionText", typeof(string), typeof(TuDuObject), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TagTextProperty =
            DependencyProperty.Register("TagText", typeof(string), typeof(TuDuObject), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DateTextProperty =
            DependencyProperty.Register("DateText", typeof(string), typeof(TuDuObject), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TuDuObject), new PropertyMetadata(false, OnIsSelectedChanged));

        public static readonly RoutedEvent AddSubItemRequestedEvent = EventManager.RegisterRoutedEvent(
            "AddSubItemRequested", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TuDuObject));

        public string DescriptionText
        {
            get { return (string)GetValue(DescriptionTextProperty); }
            set { SetValue(DescriptionTextProperty, value); }
        }

        public string TagText
        {
            get { return (string)GetValue(TagTextProperty); }
            set { SetValue(TagTextProperty, value); }
        }

        public string DateText
        {
            get { return (string)GetValue(DateTextProperty); }
            set { SetValue(DateTextProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public event RoutedEventHandler AddSubItemRequested
        {
            add { AddHandler(AddSubItemRequestedEvent, value); }
            remove { RemoveHandler(AddSubItemRequestedEvent, value); }
        }

        public event RoutedEventHandler? Selected;

        public TuDuObject()
        {
            InitializeComponent();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TuDuObject control = (TuDuObject)d;
            if ((bool)e.NewValue)
            {
                control.Selected?.Invoke(control, new RoutedEventArgs());
            }
        }

        private void MainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != SelectionCheckBox && e.OriginalSource != InputTextBox)
            {
                if (InputTextBox.IsFocused)
                {
                    ResetFocus.Focus();
                }
                else
                {
                    IsSelected = !IsSelected;
                }

                e.Handled = true;
            }
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectionCheckBox.Visibility = Visibility.Collapsed;
            MainBorder.BorderBrush = Brushes.Blue;
            InputTextBox.Text = TuDuParser.Serialize(_tuDuItem);
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectionCheckBox.Visibility = Visibility.Visible;
            MainBorder.ClearValue(Border.BorderBrushProperty);
            _tuDuItem.RawData = InputTextBox.Text;
            _tuDuItem.UpdateData();
            InputTextBox.Text = _tuDuItem.Description;
        }

        private void AddSub_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddSubItemRequestedEvent, this));
            e.Handled = true;
        }

        private void TuDuObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (e.Source == this)
            {
                if (this.DataContext is TuDuItem currentItem)
                {
                    _tuDuItem = currentItem;
                }
            }
        }
    }
}
