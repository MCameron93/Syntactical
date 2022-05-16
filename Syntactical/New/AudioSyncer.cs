using System;
using System.Diagnostics;

namespace Syntactical
{
    public class AudioSyncer
    {
        protected bool isBeat;
        private AudioSpectrum audioSpectrum;
        private double audioValue;
        private double previousAudioValue;
        private TimeSpan beatTimer;
        protected TimeSpan deltaTime;
        private Stopwatch stopwatch;

        public AudioSyncer(AudioSpectrum audioSpectrum)
        {
            this.audioSpectrum = audioSpectrum ?? throw new ArgumentNullException(nameof(audioSpectrum));
            stopwatch = new Stopwatch();
        }

        public float Bias { get; set; }
        public float RestSmoothTime { get; set; }
        public TimeSpan TimeStep { get; set; }
        public TimeSpan TimeToBeat { get; set; }

        public virtual void Beat()
        {
            beatTimer = TimeSpan.Zero;
            isBeat = true;
        }

        public virtual void Update()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            deltaTime = stopwatch.Elapsed;
            stopwatch.Restart();

            // update audio value
            previousAudioValue = audioValue;
            audioValue = audioSpectrum.SpectrumValue;

            // if audio value went below the bias during this frame
            if (previousAudioValue > Bias &&
                audioValue <= Bias)
            {
                // if minimum beat interval is reached
                if (beatTimer > TimeStep)
                    Beat();
            }

            // if audio value went above the bias during this frame
            if (previousAudioValue <= Bias &&
                audioValue > Bias)
            {
                // if minimum beat interval is reached
                if (beatTimer > TimeStep)
                    Beat();
            }

            beatTimer += deltaTime;
        }
    }
}