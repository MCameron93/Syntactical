using NAudio.Extras;
using SyntacticalPetApp.Audio;
using SyntacticalPetApp.Sprites;
using System;
using System.Collections.Generic;
using System.IO;

namespace SyntacticalPetApp
{
    public class MainWindowViewModel
    {
        private readonly SpectrumAnalyser spectrumAnalyser;
        private int updateCount;

        public MainWindowViewModel()
        {
            ProgressPanelViewModel = new ProgressPanelViewModel();

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

            var dogAnimator = new Animator();
            DogSpriteViewModel = new SpriteViewModel(dogAnimator)
            {
                Animations = dogAnimations
            };
            DogSpriteViewModel.SetAnimation("idle");

            spectrumAnalyser = new SpectrumAnalyser();

            var audioPlayback = new AudioPlayback();
            audioPlayback.FftCalculated += OnFftCalculated;

            string fileName = Path.Combine(Directory.GetCurrentDirectory(),
                "Resources", "Audio", "you_give_me_feelings.mp3");

            audioPlayback.Load(fileName);
            audioPlayback.Volume = 1;

            var animationSchedule = new AnimationSchedule(new List<AnimationTime>()
            {
                new AnimationTime("dance", TimeSpan.FromSeconds(32.08)),
                new AnimationTime("idle", TimeSpan.FromSeconds(48.10)),
                new AnimationTime("dance_b", TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(4.09)),
            });
            animationSchedule.Animate += OnAnimate;

            DogSpriteViewModel.PlayAnim();
            audioPlayback.Play();
            animationSchedule.Start();
        }

        public SpriteViewModel DogSpriteViewModel { get; set; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }

        private void OnAnimate(object sender, string e)
        {
            DogSpriteViewModel?.SetAnimation(e);
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