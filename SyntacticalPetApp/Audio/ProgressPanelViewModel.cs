using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntacticalPetApp.Audio
{
    public class ProgressPanelViewModel : INotifyPropertyChanged
    {
        public ProgressBarViewModel[] ProgressBars { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void UpdatePercentages(double[] percentages)
        {
            if (ProgressBars == null || ProgressBars.Length != percentages.Length)
            {
                ProgressBars = new ProgressBarViewModel[percentages.Length];
            }

            for (int i = 0; i < percentages.Length; i++)
            {
                if (ProgressBars[i] == null)
                {
                    ProgressBars[i] = new ProgressBarViewModel() { Label = i.ToString() };
                }
                ProgressBars[i].Value = (1 - percentages[i]) * 100;
            }
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressBars)));
        }
    }
}
