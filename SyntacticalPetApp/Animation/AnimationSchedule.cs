using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace SyntacticalPetApp
{
    public class AnimationSchedule
    {
        private Timer timer;
        private AnimationTime currentAnimation;
        private readonly Queue<AnimationTime> animationTimes = new Queue<AnimationTime>();

        public EventHandler<string> Animate;

        public AnimationSchedule(List<AnimationTime> animationTimes)
        {
            foreach (var item in animationTimes.OrderBy(t => t.Time))
            {
                this.animationTimes.Enqueue(item);
            }

            currentAnimation = this.animationTimes.Dequeue();

            timer = new Timer();
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = false;
            timer.Interval = currentAnimation.Time.TotalMilliseconds;
        }

        public void Start()
        {
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan previousTime = currentAnimation.Time;
            Animate?.Invoke(this, currentAnimation.Name);
            if (animationTimes.Any())
            {
                currentAnimation = animationTimes.Dequeue();
                timer.Interval = currentAnimation.Time.TotalMilliseconds - previousTime.TotalMilliseconds;
                timer.Start();
            }
        }
    }
}