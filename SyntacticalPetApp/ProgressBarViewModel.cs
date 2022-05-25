using System.ComponentModel;

namespace SyntacticalPetApp
{
    public class ProgressBarViewModel : INotifyPropertyChanged
    {
        public double ScaleValue { get; set; }
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

        public double Offset { get; internal set; }
        public double MinPercent { get; internal set; } = 0.0;
        public double MaxPercent { get; internal set; } = 1.0;
    }
}