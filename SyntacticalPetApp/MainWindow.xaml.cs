using NAudio.Extras;
using SyntacticalPetApp.Audio;
using System.Diagnostics;
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

        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }
        public MainWindow()
        {
            ProgressPanelViewModel = new ProgressPanelViewModel();
            DataContext = this;
            InitializeComponent();

            spectrumAnalyser = new SpectrumAnalyser();
                        
            var audioPlayback = new AudioPlayback();
            audioPlayback.FftCalculated += OnFftCalculated;
                        
            string fileName = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings_skip_intro.mp3");
            audioPlayback.Load(fileName);
            
            audioPlayback.Play();
        }

        // reduce the number of points we plot for a less jagged line?
        private int updateCount;
        private void OnFftCalculated(object sender, FftEventArgs e)
        {
            if (updateCount++ % 2 == 0)
            {
                return;
            }
            double[] percentages = spectrumAnalyser.GetPercentages(e.Result);
            
            ProgressPanelViewModel.UpdatePercentages(percentages);

            if (OldSpectrumAnalyser != null)
            {
                OldSpectrumAnalyser.Update(e.Result);
            }

            //if (ProgressPanelControl != null)
            //{
            //    ProgressPanelControl.Update(e.Result);
            //}
        }
    }
}