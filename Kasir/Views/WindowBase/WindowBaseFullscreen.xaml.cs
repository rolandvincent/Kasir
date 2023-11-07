using Kasir.Utils.Controls;
using Kasir.Utils.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace Kasir.Views.WindowBase
{
    /// <summary>
    /// Interaction logic for WindowBaseFullscreen.xaml
    /// </summary>
    public partial class WindowBaseFullscreen : Window
    {

        private bool isFullscreen = false;
        private readonly ModalDialogManager dialogManager;
        public WindowBaseFullscreen()
        {
            InitializeComponent();
            dialogManager = new ModalDialogManager(PopupContainer);
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
                TitleBarCaptionText.Margin = new Thickness(0, 0, TitleBarButtonPanel.ActualWidth, 0);
            else
                TitleBarCaptionText.Margin = new Thickness(0, 0, 0, 0);
        }

        private void ExitFullscreen_Click(object sender, RoutedEventArgs e)
        {
            TitleBar.Visibility = Visibility.Visible;
            WindowState = WindowState.Normal;
            WindowChrome.GetWindowChrome(this).CaptionHeight = 40;
            btnExitFullscreen.Visibility = Visibility.Collapsed;
            isFullscreen = false;
        }

        private bool AnimationRunning = false;
        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFullscreen)
            {
                if (Mouse.GetPosition(this).Y < 15)
                {
                    if (btnExitFullscreen.Visibility == Visibility.Collapsed)
                    {
                        btnExitFullscreen.Visibility = Visibility.Visible;
                        TranslateTransform translateTransform = new();
                        var translateYAnimation = new DoubleAnimation()
                        {
                            From = -50,
                            To = 25,
                            Duration = TimeSpan.FromSeconds(0.3)
                        };

                        btnExitFullscreen.RenderTransform = translateTransform;

                        translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
                    }
                }
                else if(Mouse.GetPosition(this).Y > 60)
                {
                    if (!btnExitFullscreen.IsMouseOver && btnExitFullscreen.Visibility == Visibility.Visible)
                    {
                        if (!AnimationRunning)
                        {
                            AnimationRunning = true;
                            TranslateTransform translateTransform = new();
                            var translateYAnimation = new DoubleAnimation()
                            {
                                From = 25,
                                To = -50,
                                Duration = TimeSpan.FromSeconds(0.2)
                            };

                            translateYAnimation.Completed += (sender, e) =>
                            {
                                btnExitFullscreen.Visibility = Visibility.Collapsed;
                                AnimationRunning = false;
                            };

                            btnExitFullscreen.RenderTransform = translateTransform;

                            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
                        }
                    }

                }
            }
        }

        private void BtnFullscreen_Click(object sender, RoutedEventArgs e)
        {
            ToogleFullscreen();
        }

        private void ToogleFullscreen()
        {
            if (isFullscreen)
            {
                TitleBar.Visibility = Visibility.Visible;
                WindowState = WindowState.Normal;
                WindowChrome.GetWindowChrome(this).CaptionHeight = 40;
                btnExitFullscreen.Visibility = Visibility.Collapsed;
                isFullscreen = false;
            }
            else
            {
                isFullscreen = true;
                dialogManager.MessageEqueue(new MessageToast()
                {
                    Content = "Tekan [F11] untuk keluar Fullscreen.",
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 20, 0, 0),
                    Opacity = 0.8,
                    Duration = TimeSpan.FromSeconds(2)
                });
                dialogManager.MessageQueueClear();
                WindowChrome.GetWindowChrome(this).CaptionHeight = 0;
                TitleBar.Visibility = Visibility.Collapsed;
                WindowState = WindowState.Maximized;
            }     
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F11) {
                ToogleFullscreen();
            }
        }
    }
}
