using Syntactical.ProgressPanel;
using System.IO;
using System.Windows;

namespace Syntactical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var audioSpectrum = new AudioSpectrum();
            var audioPlayback = new AudioPlayback();

            var progressPanelViewModel = new ProgressPanelViewModel(audioPlayback, audioSpectrum);
            MainWindowViewModel = new MainWindowViewModel(progressPanelViewModel);
            DataContext = MainWindowViewModel;

            var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings_skip_intro.mp3");
            audioPlayback.Load(audioFilePath);
            audioPlayback.Play();
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }
    }
}