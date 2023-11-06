using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kasir.Commons.Input
{
    public class DelayedEvent
    {
        private Task _task;
        private readonly TimeSpan _time;
        private readonly Stopwatch stopwatch;

        public event EventHandler EventTriggered;

        public bool IsRunning { get; private set; }

        public DelayedEvent(TimeSpan time)
        {
            _time = time;
            stopwatch = new Stopwatch();
        }

        private async void Runner()
        {
            IsRunning = true;
            while (stopwatch.ElapsedMilliseconds < _time.TotalMilliseconds)
            {
                await Task.Delay(1);
            }
            EventTriggered?.Invoke(this, EventArgs.Empty);
            stopwatch.Stop();
            stopwatch.Reset();
            IsRunning = false;
        }

        public void ResetAndStart()
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
            if (_task == null || !IsRunning)
            {
                _task = Task.Run(Runner);
            }
        }
    }
}
