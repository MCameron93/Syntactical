using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;

namespace SyntacticalPetApp.Sprites
{
    public class SpriteViewModel : INotifyPropertyChanged
    {
        private Timer animTimer;
        private Animation currentAnim;
        private int currentFrame = 0;

        public SpriteViewModel()
        {
            animTimer = new Timer();
            animTimer.Elapsed += OnAnimTimerElapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, Animation> Animations { get; set; }

        public string ImagePath
        {
            get { return currentAnim?.ImagePaths[currentFrame]; }
        }

        public void PlayAnim()
        {
            animTimer.Start();
        }

        public void SetAnimation(string animationName)
        {
            if (Animations.TryGetValue(animationName, out var animation))
            {
                currentAnim = animation;

                const int beatsPerMinute = 120;
                const int beatsPerSecond = beatsPerMinute / 60;
                const int framesPerBeat = 4;
                const int framesPerSecond = framesPerBeat * beatsPerSecond;
                const double secondsPerFrame = 1.0 / framesPerSecond;

                animTimer.Interval = TimeSpan.FromSeconds(secondsPerFrame).TotalMilliseconds;
                animTimer.Stop();
            }
        }

        private void OnAnimTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"{currentFrame}");
            currentFrame++;
            if (currentFrame >= currentAnim.Frames)
            {
                currentFrame = 0;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
        }
    }
}