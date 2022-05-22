using NAudio.Extras;
using SyntacticalPetApp.Audio;
using SyntacticalPetApp.Sprites;
using System.Collections.Generic;
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
        public SpriteViewModel DogSpriteViewModel { get; set; }
        private int updateCount;

        public MainWindow()
        {
            ProgressPanelViewModel = new ProgressPanelViewModel();

            var dogIdleAnim = new Animation()
            {
                ImagePaths = new[]
                {
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_01.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_03.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                }
            };

            var dogAnimations = new Dictionary<string, Animation>
            {
                { "idle", dogIdleAnim }
            };

            DogSpriteViewModel = new SpriteViewModel()
            {
                Animations = dogAnimations
            };
            DogSpriteViewModel.SetAnimation("idle");

            DataContext = this;
            InitializeComponent();

            spectrumAnalyser = new SpectrumAnalyser();

            var audioPlayback = new AudioPlayback();
            audioPlayback.FftCalculated += OnFftCalculated;

            string fileName = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings.mp3");
            audioPlayback.Load(fileName);

            audioPlayback.Volume = 0;
            DogSpriteViewModel.PlayAnim();
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