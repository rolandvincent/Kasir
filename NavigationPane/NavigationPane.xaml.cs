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

namespace NavigationPane
{
    /// <summary>
    /// Interaction logic for NavigationPane.xaml
    /// </summary>
    public partial class NavigationPane : UserControl
    {
        public NavigationPane()
        {
           
        }



        public Brush MenuBackground
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuBackgroundProperty =
            DependencyProperty.Register("MenuBackground", typeof(Brush), typeof(NavigationPane), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        public int PanelMinWidth
        {
            get { return (int)GetValue(PanelMinWidthProperty); }
            set { SetValue(PanelMinWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelMinWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelMinWidthProperty =
            DependencyProperty.Register("PanelMinWidth", typeof(int), typeof(NavigationPane), new PropertyMetadata(0));


        public int PanelWidth
        {
            get { return (int)GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelWidthProperty =
            DependencyProperty.Register("PanelWidth", typeof(int), typeof(NavigationPane));



        public int MenuCornerRadius
        {
            get { return (int)GetValue(MenuCornerRadiusProperty); }
            set { SetValue(MenuCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuCornerRadiusProperty =
            DependencyProperty.Register("MenuCornerRadius", typeof(int), typeof(NavigationPane), new PropertyMetadata(5));




        public int MenuItemHeight
        {
            get { return (int)GetValue(MenuItemHeightProperty); }
            set { SetValue(MenuItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemHeightProperty =
            DependencyProperty.Register("MenuItemHeight", typeof(int), typeof(NavigationPane), new PropertyMetadata(30));





    }
}
