using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kasir.Views.WindowBase
{
    /// <summary>
    /// Interaction logic for WindowBaseNormal.xaml
    /// </summary>
    public partial class WindowBaseNormal : Window
    {
        public WindowBaseNormal()
        {
            InitializeComponent();
        }
        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_MaximizeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WindowState == WindowState.Normal && ResizeMode != ResizeMode.NoResize;
        }
        private void CommandBinding_RestoreCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WindowState == WindowState.Maximized;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void root_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FormattedText formattedText = new FormattedText(Title, Thread.CurrentThread.CurrentCulture, FlowDirection.LeftToRight, new Typeface(TitleBarCaptionText.FontFamily, TitleBarCaptionText.FontStyle, TitleBarCaptionText.FontWeight, TitleBarCaptionText.FontStretch), TitleBarCaptionText.FontSize, TitleBarCaptionText.Foreground);
            if (TitleBarButtonPanel.ActualWidth + formattedText.Width / 2 > this.ActualWidth / 2)
                TitleBarCaptionText.Margin = new Thickness(0, 0, TitleBarButtonPanel.ActualWidth, 0);
            else
                TitleBarCaptionText.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}
