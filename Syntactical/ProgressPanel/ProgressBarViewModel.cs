using System.ComponentModel;

namespace Syntactical.ProgressPanel
{
    public class ProgressBarViewModel : INotifyPropertyChanged
    {
        private string label;

        private double value;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Label 
        { 
            get => label;
            set
            {
                if (value != label)
                {
                    label = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Label)));
                }
            }
        }

        public double Value
        {
            get => value;
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }
    }
}