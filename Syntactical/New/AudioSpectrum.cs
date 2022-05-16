
using NAudio.Dsp;
using System;
using System.Linq;

namespace Syntactical
{
    public class AudioSpectrum
    {
        public double SpectrumValue { get; private set; }

        public void Update(Complex[] fftResults)
        {
            if (fftResults.Any())
            {
                SpectrumValue = GetPercentage(fftResults[0]);
            }
        }

        private double GetPercentage(Complex complex)
        {
            double magnitude = Math.Sqrt(complex.X * complex.X + complex.Y * complex.Y);
            double intensityDB = 10 * Math.Log10(magnitude);
            double minDB = -90;
            if (intensityDB < minDB)
            {
                intensityDB = minDB;
            }

            double percent = intensityDB / minDB;
            return percent;
        }
    }
}