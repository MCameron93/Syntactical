using System;

namespace SyntacticalPetApp
{
    public struct AnimationTime
    {
        public AnimationTime(string name, TimeSpan time)
        {
            Name = name;
            Time = time;
        }

        public string Name { get; set; }
        public TimeSpan Time { get; set; }
    }
}
