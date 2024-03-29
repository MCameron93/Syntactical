﻿using System;
using System.ComponentModel;

namespace SyntacticalPetApp
{
    public class ProgressPanelViewModel : INotifyPropertyChanged
    {
        public ProgressPanelViewModel()
        {
            var progressBars = new ProgressBarViewModel[]
            {
                new ProgressBarViewModel { Label = "Party", MinPercent = 0.5, MaxPercent = 1.0, ScaleValue = 1.0 },
                new ProgressBarViewModel { Label = "Happiness", MinPercent = 0.6, MaxPercent = 0.8, ScaleValue = 0.5 },
                new ProgressBarViewModel { Label = "Hungy", Value = 100, MinPercent = 0.5, MaxPercent = 1.0, ScaleValue = 0.25 },
                new ProgressBarViewModel { Label = "Sleepy", MinPercent = 0.5, MaxPercent = 1.0, ScaleValue = 0.25 },
            };

            Bars = progressBars;
        }

        public ProgressBarViewModel[] Bars { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void UpdatePercentages(double[] percentages)
        {
            for (int i = 0; i < percentages.Length; i++)
            {
                double min = Bars[i].MinPercent;
                double max = Bars[i].MaxPercent;
                double input = 1 - percentages[i];
                double val = (input - min) / (max - min);
                double scaleValue = Bars[i].ScaleValue;

                Bars[i].Value = Math.Max(5, val * scaleValue * 100);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bars)));
        }
    }
}