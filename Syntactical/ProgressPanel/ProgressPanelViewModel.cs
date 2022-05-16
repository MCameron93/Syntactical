using NAudio.Extras;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;

namespace Syntactical.ProgressPanel
{
    public class ProgressPanelViewModel
    {
        private Timer timer;
        private AudioPlayback audioPlayback;
        private AudioSpectrum audioSpectrum;

        private AudioSyncerScale[] audioSyncerScales;
        public ProgressPanelViewModel(AudioPlayback audioPlayback, AudioSpectrum audioSpectrum)
        {
            this.audioPlayback = audioPlayback ?? throw new ArgumentNullException(nameof(audioPlayback));
            this.audioSpectrum = audioSpectrum ?? throw new ArgumentNullException(nameof(audioSpectrum));

            audioPlayback.FftCalculated += OnFftCalculated;

            audioSyncerScales = new AudioSyncerScale[2];
            ProgressBars = new ProgressBarViewModel[2];

            audioSyncerScales[0] = new AudioSyncerScale(audioSpectrum)
            {
                Bias = 0.25f,
                TimeStep = TimeSpan.FromSeconds(0.4),
                TimeToBeat = TimeSpan.FromSeconds(0.05),
                RestSmoothTime = 0.005f,
                BeatPercent = 100.0,
                RestPercent = 0.0
            };

            audioSyncerScales[1] = new AudioSyncerScale(audioSpectrum)
            {
                Bias = 0.25f,
                TimeStep = TimeSpan.FromSeconds(0.4),
                TimeToBeat = TimeSpan.FromSeconds(0.05),
                RestSmoothTime = 0.005f,
                BeatPercent = 85.0,
                RestPercent = 0.0
            };

            for (int i = 0; i < 2; i++)
            {
                ProgressBars[i] = new ProgressBarViewModel() { Label = $"{i}", Value = ((double)i / 16) * 100 };
                audioSyncerScales[i].ProgressBarViewModel = ProgressBars[i];
            }

            timer = new Timer
            {
                Interval = TimeSpan.FromMilliseconds(10).TotalMilliseconds
            };
            timer.Elapsed += OnUpdateTimerElapsed;
            timer.Start();
        }

        private void OnFftCalculated(object sender, FftEventArgs e)
        {
            audioSpectrum.Update(e.Result);
        }

        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                //audioSyncerScales[i].Update();
                //ProgressBars[i].Value = audioSyncerScales[i].Percent;
                ProgressBars[i].Value = audioSpectrum.SpectrumValue * 100;
            }
        }


        public ProgressBarViewModel[] ProgressBars { get; }
    }
}