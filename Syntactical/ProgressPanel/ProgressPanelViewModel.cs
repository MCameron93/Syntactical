using System;
using System.ComponentModel;
using System.Timers;

namespace Syntactical.ProgressPanel
{
    public class ProgressPanelViewModel : INotifyPropertyChanged
    {
        private double boneCycleProgress = 0;
        private readonly Timer boneCycleTimer;

        private readonly Timer sampleTimer;

        public ProgressBarViewModel[] ProgressBars { get; }

        public ProgressPanelViewModel()
        {
            ProgressBars = new ProgressBarViewModel[16];
            for (int i = 0; i < 16; i++)
            {
                ProgressBars[i] = new ProgressBarViewModel() { Label = $"{i}", Value = ((double)i / 16) * 100 };
            }

            boneCycleTimer = new Timer
            {
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(0.01).TotalMilliseconds,
            };
            boneCycleTimer.Elapsed += OnCycleTimerElapsed;
            boneCycleTimer.Start();
        }

        private void OnSampleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCycleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            BoneCycleProgress += 1;
        }

        ~ProgressPanelViewModel()
        {
            boneCycleTimer.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double BoneCycleProgress
        {
            get => boneCycleProgress;
            set
            {
                if (value != boneCycleProgress)
                {
                    boneCycleProgress = value;

                    if (boneCycleProgress > 100)
                    {
                        boneCycleProgress = 0;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoneCycleProgress)));
                }
            }
        }
    }
}