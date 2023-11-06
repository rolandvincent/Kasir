using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kasir.Commons.Input
{
    internal class TextBoxInputDelay : IDisposable
    {
        private readonly TimeSpan _time;
        private readonly Stopwatch stopwatch;
        private TextChangedEventArgs _event;
        private readonly TextBoxBase _sender;
        private Task _task;

        public bool IsRunning { get; private set; }

        /// <summary>
        /// New input delay
        /// </summary>
        /// <param name="textbox">Textbox</param>
        /// <param name="delay">Delay time</param>

        public TextBoxInputDelay(TextBoxBase textbox, TimeSpan time)
        {
            _time = time;
            _sender = textbox;
            stopwatch = new Stopwatch();

            textbox.TextChanged += Textbox_TextChanged;
        }

        private async void Runner()
        {
            IsRunning = true;
            while (stopwatch.ElapsedMilliseconds < _time.TotalMilliseconds)
            {
                await Task.Delay(1);
            }
            _sender.Dispatcher?.Invoke(() =>
            {
                InputDelayChanged?.Invoke(_sender, _event);
            });
            stopwatch.Stop();
            stopwatch.Restart();
            IsRunning = false;
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                stopwatch.Start();
            }
            _event = e;
            if (_task == null || !IsRunning)
            {
                _task = Task.Run(Runner);
            }
        }

        public void Dispose()
        {
            _sender.TextChanged -= Textbox_TextChanged;
            _task.Dispose();
            stopwatch.Stop();
        }

        /// <summary>
        /// Trigger when text changed with delay
        /// </summary>
        public event TextChangedEventHandler InputDelayChanged;
    }
}
