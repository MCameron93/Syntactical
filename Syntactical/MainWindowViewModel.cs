using NAudio.Extras;
using Syntactical.ProgressPanel;
using System;
using System.ComponentModel;
using System.IO;
using System.Timers;
using System.Windows.Input;

namespace Syntactical
{
    public class MainWindowViewModel
    {
        private AudioPlayback audioPlayback;
        private readonly SpectrumAnalyser spectrumAnalyser;

        public MainWindowViewModel(ProgressPanelViewModel progressPanelViewModel)
        {
            PlayCommand = new RelayCommand(Play);
            ProgressPanelViewModel = progressPanelViewModel ?? throw new ArgumentNullException(nameof(progressPanelViewModel));
        }

        public ICommand PlayCommand { get; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }
        public AudioSpectrum AudioSpectrum { get; set; }

        private void audioGraph_FftCalculated(object sender, FftEventArgs e)
        {
            if (spectrumAnalyser != null)
            {
                spectrumAnalyser.Update(e.Result);
                AudioSpectrum.Update(e.Result);
            }    
        }

        private void audioGraph_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
        }

        private void Play()
        {
            audioPlayback = new AudioPlayback();
            audioPlayback.MaximumCalculated += audioGraph_MaximumCalculated;
            audioPlayback.FftCalculated += audioGraph_FftCalculated;

            var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings.mp3");
            audioPlayback.Load(audioFilePath);
            audioPlayback.Play();
        }
    }
}