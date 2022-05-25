using NAudio.Extras;
using SyntacticalPetApp.Audio;
using SyntacticalPetApp.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SyntacticalPetApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SpectrumAnalyser spectrumAnalyser;
        private readonly AudioPlayback audioPlayback;
        private readonly AnimationSchedule dogAnimationScheduler;
        private int updateCount;

        public MainWindowViewModel()
        {
            spectrumAnalyser = new SpectrumAnalyser();
            ProgressPanelViewModel = new ProgressPanelViewModel();
            DogCommandsViewModel = new DogCommandsViewModel
            {
                ProgressPanelViewModel = ProgressPanelViewModel
            };
            DogCommandsViewModel.PartyModeEntered += OnPartyModeEntered;
            var dogAnimator = new Animator();
            Dictionary<string, Animation> dogAnimations = GetDogAnimations();
            DogSpriteViewModel = new SpriteViewModel(dogAnimator) { Animations = dogAnimations };
            DogSpriteViewModel.SetAnimation("idle");

            dogAnimationScheduler = new AnimationSchedule(new[]
            {
                new AnimationTime("dance", TimeSpan.FromSeconds(32.08)),
                new AnimationTime("idle", TimeSpan.FromSeconds(48.10)),
                new AnimationTime("dance_b", TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(4.09)),
            });
            dogAnimationScheduler.Animate += OnDogAnimate;

            // Initialise audio playback.
            string fileName = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings.mp3");

            audioPlayback = new AudioPlayback();
            audioPlayback.FftCalculated += OnFftCalculated;
            audioPlayback.Load(fileName);
            audioPlayback.Volume = 1;

        }

        private void OnPartyModeEntered(object sender, EventArgs e)
        {
            // Start all animations, audio, timers, etc.
            DogSpriteViewModel.PlayAnim();
            audioPlayback.Play();
            dogAnimationScheduler.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DogCommandsViewModel DogCommandsViewModel { get; set; }
        public SpriteViewModel DogSpriteViewModel { get; set; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }

        private static Dictionary<string, Animation> GetDogAnimations()
        {
            // Work out how many seconds there are between each frame based on beats per minute of
            // the song. We know this is 120 bpm for the song being used here.
            const int beatsPerMinute = 120;
            const int beatsPerSecond = beatsPerMinute / 60;

            // Frames per beat is decided per animation. i.e. How many frames of animation should
            // there be between each beat in the song.
            double danceSecondsPerFrame = GetSecondsPerFrame(framesPerBeat: 2);
            double idleSecondsPerFrame = GetSecondsPerFrame(framesPerBeat: 4);

            double GetSecondsPerFrame(int framesPerBeat)
            {
                int framesPerSecond = framesPerBeat * beatsPerSecond;
                double secondsPerFrame = 1.0 / framesPerSecond;
                return secondsPerFrame;
            }

            var dogDanceAnim = new Animation()
            {
                FramePaths = new[]
                {
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_02.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_01.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_03.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_01.png"
                },
                TimeBetweenFrames = TimeSpan.FromSeconds(danceSecondsPerFrame)
            };

            var dogDanceBAnim = new Animation()
            {
                FramePaths = new[]
                {
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_b_02.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_b_01.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_b_03.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_dance_b_01.png"
                },
                TimeBetweenFrames = TimeSpan.FromSeconds(danceSecondsPerFrame)
            };

            var dogIdleAnim = new Animation()
            {
                FramePaths = new[]
                {
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_01.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_03.png",
                    "/SyntacticalPetApp;component/Resources/Art/zach_idle_02.png",
                },
                TimeBetweenFrames = TimeSpan.FromSeconds(idleSecondsPerFrame)
            };

            var dogAnimations = new Dictionary<string, Animation>
            {
                { "idle", dogIdleAnim },
                { "dance", dogDanceAnim },
                { "dance_b", dogDanceBAnim }
            };
            return dogAnimations;
        }

        private void OnDogAnimate(object sender, string animationName)
        {
            DogSpriteViewModel?.SetAnimation(animationName);
            DogSpriteViewModel?.PlayAnim();
        }

        private void OnFftCalculated(object sender, FftEventArgs e)
        {
            if (updateCount++ % 2 == 0)
            {
                //return;
            }

            double[] percentages = spectrumAnalyser.GetPercentages(e.Result,
                samples: ProgressPanelViewModel.ProgressBars.Length, pointsPerSample: 1);

            ProgressPanelViewModel.UpdatePercentages(percentages);

            //if (OldSpectrumAnalyser != null)
            //{
            //    OldSpectrumAnalyser.Update(e.Result);
            //}
        }
    }
}