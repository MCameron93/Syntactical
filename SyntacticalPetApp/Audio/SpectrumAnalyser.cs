using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntacticalPetApp.Audio
{
    public class SpectrumAnalyser
    {
        private const int binsPerPoint = 128;
        private int bins = 512;

        public SpectrumAnalyser()
        {
        }

        public double[] GetPercentages(Complex[] fftResults, int samples, int pointsPerSample)
        {
            double[] percentages = new double[samples];
            int binCount = fftResults.Length / samples;

            for (int i = 0; i < percentages.Length; i++)
            {
                double percentageTotals = 0;
                for (int sampleIndex = 0; sampleIndex < pointsPerSample; sampleIndex++)
                {
                    percentageTotals += GetPercentage(fftResults[i * samples + sampleIndex]);
                }
                percentages[i] = percentageTotals / pointsPerSample;
            }
            //int newBins = fftResults.Length / samples;

            //for (int i = 0; i < newBins; i+=samples)
            //{
            //    double percentageTotals = 0;
            //    for (int n = 0; n < pointsPerSample; n++)
            //    {
            //        int fftIndex = i * pointsPerSample + n;
            //        Complex complex = fftResults[fftIndex];
            //        percentageTotals += GetPercentage(complex);
            //    }

            //    double averagePercent = percentageTotals / pointsPerSample;
            //    percentages[i] = averagePercent;
            //}

            return percentages;
        }

        internal double[] GetPercentages(Complex[] fftResults)
        {
            if (fftResults.Length / 2 != bins)
            {
                bins = fftResults.Length / 2;
            }

            double[] percentages = new double[bins];

            for (int n = 0; n < fftResults.Length / 2; n += binsPerPoint)
            {
                // averaging out bins
                double percentageTotals = 0;
                for (int b = 0; b < binsPerPoint; b++)
                {
                    percentageTotals += GetPercentage(fftResults[n + b]);
                }

                int index = n / binsPerPoint;
                double averagePercent = percentageTotals / binsPerPoint;
                percentages[index] = averagePercent;
            }

            return percentages;
        }

        private double GetPercentage(Complex complex)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double magnitude = Math.Sqrt(complex.X * complex.X + complex.Y * complex.Y);
            double intensityDB = 10 * Math.Log10(magnitude);
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            double yPos = percent;
            return yPos;
        }
    }
}
