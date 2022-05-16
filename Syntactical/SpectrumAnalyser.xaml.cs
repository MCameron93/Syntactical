using NAudio.Dsp;
using Syntactical.ProgressPanel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Syntactical
{
    /// <summary>
    /// Interaction logic for SpectrumAnalyser.xaml
    /// </summary>
    public partial class SpectrumAnalyser : UserControl
    {
        private const int binsPerPoint = 2;
        private int bins = 512;

        // reduce the number of points we plot for a less jagged line?
        private int updateCount;

        private double xScale = 200;
        // guess a 1024 size FFT, bins is half FFT size

        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }
        public SpectrumAnalyser()
        {
            InitializeComponent();

            this.ProgressPanelControl.DataContext = ProgressPanelViewModel;

            CalculateXScale();
            SizeChanged += SpectrumAnalyser_SizeChanged;
        }

        public void Update(Complex[] fftResults)
        {
            // no need to repaint too many frames per second
            if (updateCount++ % 2 == 0)
            {
                return;
            }

            if (fftResults.Length / 2 != bins)
            {
                bins = fftResults.Length / 2;
                CalculateXScale();
            }

            for (int n = 0; n < fftResults.Length / 2; n += binsPerPoint)
            {
                // averaging out bins
                double yPos = 0;
                for (int b = 0; b < binsPerPoint; b++)
                {
                    yPos += GetYPosLog(fftResults[n + b]);
                }
                AddResult(n / binsPerPoint, yPos / binsPerPoint);
            }
        }

        private void AddResult(int index, double power)
        {
            Point p = new Point(CalculateXPos(index), power);
            if (index >= polyline1.Points.Count)
            {
                polyline1.Points.Add(p);
                
            }
            else
            {
                polyline1.Points[index] = p;

                if (index % 16 == 0)
                {
                    int pIndex = index / 16;
                    if (pIndex < ProgressPanelViewModel.ProgressBars.Length)
                    {
                        ProgressPanelViewModel.ProgressBars[pIndex].Value = 50 - (p.Y / 4);
                    }
                }
                
            }
        }

        private double CalculateXPos(int bin)
        {
            if (bin == 0) return 0;
            return bin * xScale; // Math.Log10(bin) * xScale;
        }

        private void CalculateXScale()
        {
            xScale = ActualWidth / (bins / binsPerPoint);
        }

        private double GetYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            double yPos = percent * ActualHeight;
            return yPos;
        }

        private void SpectrumAnalyser_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateXScale();
        }
    }
}