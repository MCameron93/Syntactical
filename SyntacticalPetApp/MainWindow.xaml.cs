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
                "Resources", "Audio", "you_give_me_feelings.mp3");
            audioPlayback.Load(fileName);
            
            audioPlayback.Play();
        }

        // reduce the number of points we plot for a less jagged line?
        private int updateCount;
        private void OnFftCalculated(object sender, FftEventArgs e)
        {
            // Make custom panel with a few progress bars. 
            // On update, get new fft values.
            // Assign values to specific progress bars.
            // Assign scaler to progress bar view model.
            // Compute display percentage based on scaler value and actual percentage.

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

            //if (ProgressPanelControl != null)
            //{
            //    ProgressPanelControl.Update(e.Result);
            //}
        }
    }
}