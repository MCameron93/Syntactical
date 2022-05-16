using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Syntactical
{
    public class AudioSyncerScale : AudioSyncer
    {
        private readonly CancellationTokenSource tokenSource;
        public ProgressPanel.ProgressBarViewModel ProgressBarViewModel { get; set; }

        public AudioSyncerScale(AudioSpectrum audioSpectrum) : base(audioSpectrum)
        {
            tokenSource = new CancellationTokenSource();
        }

        public double BeatPercent { get; set; }
        public double Percent { get; private set; }
        public double RestPercent { get; set; }

        private Task task;
        public override void Beat()
        {
            base.Beat();

            if (task != null && task.Status.Equals(TaskStatus.Running))
            {
                tokenSource.Cancel();
            }

            task = Task.Run(() =>
            {
                MoveToScale(BeatPercent);
            }, tokenSource.Token);
        }

        public override void Update()
        {
            base.Update();

            if (isBeat)
            {
                return;
            }

            Percent = Lerp(Percent, RestPercent, RestSmoothTime * deltaTime.TotalMilliseconds);

            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    ProgressBarViewModel.Value = Percent;
            //});
        }

        private static double Lerp(double value1, double value2, double amount)
        {
            if (amount >= 1.0f)
            {
                return value2;
            }
            return value1 * (1 - amount) + value2 * amount;
        }

        private void MoveToScale(double _target)
        {
            double _curr = Percent;
            double _initial = _curr;
            TimeSpan _timer = TimeSpan.Zero;

            while (_curr != _target)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                }

                _curr = Lerp(_initial, _target, (double)(_timer.TotalMilliseconds / TimeToBeat.TotalMilliseconds));
                _timer += deltaTime;

                Percent = _curr;
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    ProgressBarViewModel.Value = Percent;
                //});
            }

            isBeat = false;
        }
    }
}