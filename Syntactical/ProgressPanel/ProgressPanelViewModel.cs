using System.ComponentModel;

namespace Syntactical.ProgressPanel
{
    public class ProgressPanelViewModel : INotifyPropertyChanged
    {
        public ProgressPanelViewModel()
        {
            ProgressBars = new ProgressBarViewModel[16];
            for (int i = 0; i < 16; i++)
            {
                ProgressBars[i] = new ProgressBarViewModel() { Label = $"{i}", Value = ((double)i / 16) * 100 };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProgressBarViewModel[] ProgressBars { get; }
    }
}