using Kasir.ViewModels;
using Kasir.Views.WindowBase;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kasir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //TextBoxInputDelay searchBox = new TextBoxInputDelay(adawdaw, TimeSpan.FromMilliseconds(500));
            //searchBox.InputDelayChanged += SearchBox_InputDelayChanged;
            
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_MaximizeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WindowState == WindowState.Normal;
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
                TitleBarCaptionText.Margin = new Thickness(0,0,TitleBarButtonPanel.ActualWidth,0);
            else
                TitleBarCaptionText.Margin = new Thickness(0, 0, 0, 0);
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu ProfileContextMenu = FindResource("ProfileContextMenu") as ContextMenu;
            ProfileContextMenu.PlacementTarget = BtnProfile;
            ProfileContextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowBaseNormal baseNormal = new WindowBaseNormal();
            baseNormal.Width = 460;
            baseNormal.Height = 460;
            baseNormal.Title = "Login";
            baseNormal.Pages.Content = new LoginVM();
            baseNormal.Show();
        }

    }
}
