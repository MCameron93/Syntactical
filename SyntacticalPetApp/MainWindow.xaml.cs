using NAudio.Extras;
using SyntacticalPetApp.Audio;
using System.IO;
using System.Windows;

namespace SyntacticalPetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpectrumAnalyser spectrumAnalyser;

        private int updateCount;

        public MainWindow()
        {
            ProgressPanelViewModel = new ProgressPanelViewModel();

            DataContext = this;
            InitializeComponent();

            spectrumAnalyser = new SpectrumAnalyser();

            var audioPlayback = new AudioPlayback();
            audioPlayback.FftCalculated += OnFftCalculated;

            string fileName = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings.mp3");
            audioPlayback.Load(fileName);

            audioPlayback.Play();
        }

        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }

        private void OnFftCalculated(object sender, FftEventArgs e)
        {
            if (updateCount++ % 2 == 0)
            {
                //return;
            }

            double[] percentages = spectrumAnalyser.GetPercentages(e.Result,
                samples: ProgressPanelViewModel.ProgressBars.Length, pointsPerSample: 1);

            ProgressPanelViewModel.UpdatePercentages(percentages);

            if (OldSpectrumAnalyser != null)
            {
                OldSpectrumAnalyser.Update(e.Result);
            }
        }
    }
}