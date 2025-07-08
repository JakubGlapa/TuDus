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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TuDus.CustomElements
{
    /// <summary>
    /// Interaction logic for IndentationObject.xaml
    /// </summary>
    public partial class IndentationObject : UserControl
    {
        public static readonly DependencyProperty IndentWidthProperty =
            DependencyProperty.Register("IndentWidth", typeof(double), typeof(IndentationObject), new PropertyMetadata(0.0, OnIndentWidthChanged));

        public double IndentWidth
        {
            get { return (double)GetValue(IndentWidthProperty); }
            set { SetValue(IndentWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(IndentationObject), new PropertyMetadata(null, OnContentChanged));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public IndentationObject()
        {
            InitializeComponent();
        }

        private static void OnIndentWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IndentationObject control = (IndentationObject)d;
            control.IndentationColumn.Width = new GridLength((double)e.NewValue);
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IndentationObject control = (IndentationObject)d;
            if (e.OldValue is UIElement oldElement)
            {
                control.MainGrid.Children.Remove(oldElement);
            }

            if (e.NewValue is UIElement newElement)
            {
                Grid.SetColumn(newElement, 1);
                control.MainGrid.Children.Add(newElement);
            }
        }
    }
}
