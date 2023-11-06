using Kasir.Commons.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kasir.Utils.Controls
{
    public class MessageToast : ContentControl, IDisposable
    {
        static MessageToast()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageToast), new FrameworkPropertyMetadata(typeof(MessageToast)));
        }

        Stopwatch stopwatch = new Stopwatch();

        public ICommand DismissCommand { get; }
        public event EventHandler ToastCloseAnimationFinished;
        public event EventHandler ToastClick;

        public MessageToast()
        {
            Opacity = 0;
            DismissCommand = new RelayCommand(Dismiss);

            this.Loaded += MessageToast_Loaded;
            this.MouseEnter += MessageToast_MouseEnter;
            this.MouseLeave += MessageToast_MouseLeave;
            this.MouseDown += MessageToast_MouseDown;
            this.MouseUp += MessageToast_MouseUp;
        }

        private void MessageToast_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsPressed)
                ToastClick?.Invoke(this, new EventArgs());
            IsPressed = false;
        }

        private bool IsPressed = false;

        private void MessageToast_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = e.LeftButton == MouseButtonState.Pressed;
        }

        bool Suspend = false;

        private void MessageToast_MouseLeave(object sender, MouseEventArgs e)
        {
            Suspend = false;
            IsPressed = false;
        }

        private void MessageToast_MouseEnter(object sender, MouseEventArgs e)
        {
            Suspend = true;
        }
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private void MessageToast_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpan MessageDuration = Duration;

            _ = Task.Factory.StartNew(async () =>
            {
                await Dispatcher.BeginInvoke(Showing);
                await Task.Delay(MessageDuration);
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    return;
                while (Suspend)
                    await Task.Delay(1);
                await Dispatcher.BeginInvoke(Dismiss, new object[] { null });
            }, cancellationTokenSource.Token);
        }

        public async void Dismiss(object view = null)
        {
            cancellationTokenSource.Cancel();
            stopwatch.Restart();
            stopwatch.Start();

            while (stopwatch.Elapsed < AnimationDuration)
            {
                this.Opacity = 1- (stopwatch.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds);
                await Task.Delay(1);
            }
            stopwatch.Stop();
            this.Opacity = 0;
            ToastCloseAnimationFinished?.Invoke(this, new EventArgs());
        }

        public async void Showing()
        {
            stopwatch.Restart();
            stopwatch.Start();

            while (stopwatch.Elapsed < AnimationDuration)
            {
                this.Opacity = stopwatch.ElapsedMilliseconds / AnimationDuration.TotalMilliseconds;
                await Task.Delay(1);
            }
            stopwatch.Stop();
            this.Opacity = 1;
        }

        public override void OnApplyTemplate()
        {
            Showing();
            base.OnApplyTemplate();
        }

        public void Dispose()
        {
            ToastCloseAnimationFinished -= ToastCloseAnimationFinished;
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MessageToast), new PropertyMetadata(new CornerRadius(5)));

        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(MessageToast), new PropertyMetadata(TimeSpan.FromSeconds(0.2)));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(MessageToast), new PropertyMetadata(TimeSpan.FromSeconds(4)));

        public Visibility CloseButtonVisibility
        {
            get { return (Visibility)GetValue(CloseButtonVisibilityProperty); }
            set { SetValue(CloseButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonVisibilityProperty =
            DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(MessageToast), new PropertyMetadata(Visibility.Collapsed));

    }
}
