using System;
using System.Timers;

namespace SyntacticalPetApp.Sprites
{
    public class Animator
    {
        public EventHandler FrameChanged;

        private readonly Timer frameTimer;
        private Animation currentAnimation;
        private int currentFrame = 0;

        public Animator()
        {
            frameTimer = new Timer();
            frameTimer.Elapsed += FrameTimer_Elapsed;
        }

        public string CurrentFramePath => currentAnimation.FramePaths[currentFrame];

        public void SetAnimation(Animation animation)
        {
            currentAnimation = animation;
            frameTimer.Interval = animation.TimeBetweenFrames.TotalMilliseconds;
            frameTimer.Stop();
        }

        internal void Play()
        {
            frameTimer.Start();
        }

        private void FrameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentFrame++;
            if (currentFrame >= currentAnimation.Frames)
            {
                currentFrame = 0;
            }
            FrameChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}