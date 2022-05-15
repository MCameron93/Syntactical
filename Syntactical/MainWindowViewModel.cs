using NAudio.Extras;
using Syntactical.ProgressPanel;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Syntactical
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private AudioPlayback audioPlayback;
        private readonly SpectrumAnalyser spectrumAnalyser;

        public MainWindowViewModel(ProgressPanelViewModel progressPanelViewModel, SpectrumAnalyser spectrumAnalyser)
        {
            PlayCommand = new RelayCommand(Play);
            ProgressPanelViewModel = progressPanelViewModel ?? throw new ArgumentNullException(nameof(progressPanelViewModel));
            this.spectrumAnalyser = spectrumAnalyser;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PlayCommand { get; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }

        public float FftDisplay { get; set; }

        private void audioGraph_FftCalculated(object sender, FftEventArgs e)
        {
            if (spectrumAnalyser != null)
            {
                spectrumAnalyser.Update(e.Result);
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