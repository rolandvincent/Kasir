using Kasir.Commons.Commands;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Kasir.Utils.Controls
{
    public class PopupModal : ContentControl, IDisposable
    {

        public event EventHandler PopupClosed;
        public event EventHandler PopupCloseAnimationFinished;
        public event EventHandler PopupShowing;
        public ICommand DismissCommand {  get; }

        private readonly TimeSpan AnimationDuration = TimeSpan.FromSeconds(0.5);

        static PopupModal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupModal), new FrameworkPropertyMetadata(typeof(PopupModal)));
        }

        public PopupModal()
        {
            DismissCommand = new RelayCommand(Dismiss);

            PopupClosed += PopupModal_PopupClosed;
            PopupShowing += PopupModal_PopupShowing;
        }

        private async void PopupModal_PopupShowing(object? sender, EventArgs e)
        {
            base.Opacity = 0;       
            Visibility = Visibility.Visible;

            while (GetTemplateChild("InnerContent") as FrameworkElement == null)
            {
                await Task.Delay(1);
            }

            _timer.Reset();
            _timer.Start();
            FrameworkElement InnerContent = GetTemplateChild("InnerContent") as FrameworkElement;

            var be = new PowerEase();
           
            while (_timer.ElapsedMilliseconds < AnimationDuration.TotalMilliseconds)
            {
                base.Opacity = _timer.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds * this.Opacity;
                double scale = be.Ease(_timer.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds)/2 + 0.5;
                if (InnerContent != null)
                InnerContent.RenderTransform = new ScaleTransform(scale, scale);
                await Task.Delay(1);
            }
            base.Opacity = this.Opacity;
        }


        Stopwatch _timer = new Stopwatch();
        private async void PopupModal_PopupClosed(object? sender, EventArgs e)
        {
            _timer.Reset();
            _timer.Start();
            FrameworkElement InnerContent = GetTemplateChild("InnerContent") as FrameworkElement;
            var be = new SineEase();

            while (_timer.ElapsedMilliseconds < AnimationDuration.TotalMilliseconds)
            {
                base.Opacity = (1 - (_timer.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds)) * this.Opacity;
                double scale = be.Ease(1- _timer.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds) / 2 + 0.5;
                if (InnerContent != null)
                    InnerContent.RenderTransform = new ScaleTransform(scale, scale);
                await Task.Delay(1);
            }
            Visibility = Visibility.Collapsed;
            PopupCloseAnimationFinished?.Invoke(this, new EventArgs());
        }

        private void Dismiss(object view) => IsOpen = false;

        public void Dispose()
        {
            PopupClosed -= PopupModal_PopupClosed;
            PopupShowing -= PopupModal_PopupShowing;
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set {
                var last = IsOpen;
                SetValue(IsOpenProperty, value); 
                if (last != value)
                {
                    if (!value)
                        PopupClosed?.Invoke(this, EventArgs.Empty);
                    else
                        PopupShowing?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupModal), new PropertyMetadata(false));

        public new double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Opacity.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(PopupModal), new PropertyMetadata(1d));



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PopupModal), new PropertyMetadata("Message Popup"));
    }
}
