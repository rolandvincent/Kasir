using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kasir.Utils.Controls
{
    public class PlaceholderTextBox : TextBox
    {
        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(PlaceholderTextBox)));
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            PlaceholderVisibility = Visibility.Collapsed;
            base.OnGotFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                PlaceholderVisibility = Visibility.Visible;
            else
                PlaceholderVisibility = Visibility.Collapsed;
            base.OnTextChanged(e);  
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                PlaceholderVisibility = Visibility.Visible;
            }
            base.OnLostFocus(e);
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(""));

        public Brush PlaceholderForeground
        {
            get { return (Brush)GetValue(PlaceholderForegroundProperty); }
            set { SetValue(PlaceholderForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.Register("PlaceholderForeground", typeof(Brush), typeof(PlaceholderTextBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(217,217,217))));

        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register("PlaceholderVisibility", typeof(Visibility), typeof(PlaceholderTextBox), new PropertyMetadata(Visibility.Visible));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PlaceholderTextBox), new PropertyMetadata(new CornerRadius(0)));

    }
}
