using System;
using System.ComponentModel;
using System.Timers;

namespace SyntacticalPetApp.Sprites
{
    public class SpriteViewModel : INotifyPropertyChanged
    {
        public static readonly TimeSpan TimeBetweenFrames = TimeSpan.FromSeconds(1);

        private Timer animTimer;
        private int currentFrame = 0;

        private string[] sprits =
        {
            "/SyntacticalPetApp;component/Resources/Art/zach_spritesheet0.png",
            "/SyntacticalPetApp;component/Resources/Art/zach_spritesheet1.png"
        };

        public SpriteViewModel()
        {
            animTimer = new Timer();
            animTimer.Interval = TimeBetweenFrames.TotalMilliseconds;
            animTimer.Elapsed += OnAnimTimerElapsed;
            animTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ImagePath
        {
            get { return sprits[currentFrame]; }
        }

        private void OnAnimTimerElapsed(object sender, ElapsedEventArgs e)
        {
            currentFrame++;
            if (currentFrame >= sprits.Length)
            {
                currentFrame = 0;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
        }
    }
}