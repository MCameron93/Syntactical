using NAudio.Extras;
using SyntacticalPetApp.Audio;
using SyntacticalPetApp.Sprites;
using System;
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

            // Work out how many seconds there are between each frame based on beats per minute of
            // the song. We know this is 120 bpm for the song being used here.
            const int beatsPerMinute = 120;
            const int beatsPerSecond = beatsPerMinute / 60;
            
            // Frames per beat is decided per animation. i.e. How many frames of animation should
            // there be between each beat in the song.
            const int idleFramesPerBeat = 2;

            const int idleFramesPerSecond = idleFramesPerBeat * beatsPerSecond;
            const double idleSecondsPerFrame = 1.0 / idleFramesPerSecond;

            var dogIdleAnim = new Animation()
            {
                FramePaths = new[]
                {
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_02.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_01.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_03.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_01.png",
                    //"/SyntacticalPetApp;component/Resources/Art/zach_idle_01.png",
                    //"/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                    //"/SyntacticalPetApp;component/Resources/Art/zach_idle_03.png",
                    //"/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                },
                TimeBetweenFrames = TimeSpan.FromSeconds(idleSecondsPerFrame)
            };

            var dogAnimations = new Dictionary<string, Animation>
            {
                { "idle", dogIdleAnim }
            };

            var dogAnimator = new Animator();
            DogSpriteViewModel = new SpriteViewModel(dogAnimator)
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

            audioPlayback.Volume = 1;
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