using System;

namespace SyntacticalPetApp.Sprites
{
    public class Animation
    {
        public string[] FramePaths { get; set; }
        public int Frames => FramePaths.Length;
        public TimeSpan TimeBetweenFrames { get; set; }
    }
}