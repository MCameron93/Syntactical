using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;

namespace SyntacticalPetApp.Sprites
{
    public class SpriteViewModel : INotifyPropertyChanged
    {
        public static readonly TimeSpan TimeBetweenFrames = TimeSpan.FromSeconds(1);

        private Timer animTimer;
        private Animation currentAnim;
        private int currentFrame = 0;

        public SpriteViewModel()
        {
            animTimer = new Timer();
            animTimer.Interval = TimeBetweenFrames.TotalMilliseconds;
            animTimer.Elapsed += OnAnimTimerElapsed;
            animTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, Animation> Animations { get; set; }

        public string ImagePath
        {
            get { return currentAnim?.ImagePaths[currentFrame]; }
        }

        public void SetAnimation(string animationName)
        {
            if (Animations.TryGetValue(animationName, out var animation))
            {
                currentAnim = animation;
            }
        }

        private void OnAnimTimerElapsed(object sender, ElapsedEventArgs e)
        {
            currentFrame++;
            if (currentFrame >= currentAnim.Frames)
            {
                currentFrame = 0;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
        }
    }
}