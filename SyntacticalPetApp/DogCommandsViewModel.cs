using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SyntacticalPetApp
{
    public class DogCommandsViewModel : INotifyPropertyChanged
    {
        public DogCommandsViewModel()
        {
            FeedCommand = new RelayCommand(_ => Feed());
            PartyCommand = new RelayCommand(_ => EnterPartyMode(), _ => !PartyModeEnabled);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand FeedCommand { get; }
        public ICommand PartyCommand { get; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }
        private bool PartyModeEnabled { get; set; }

        public EventHandler PartyModeEntered;
        private void EnterPartyMode()
        {
            MessageBox.Show(messageBoxText: "Party mode has not been implemented yet.", caption: "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            
            PartyModeEnabled = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartyModeEnabled)));
            PartyModeEntered?.Invoke(this, EventArgs.Empty);
        }

        private void Feed()
        {
            ProgressPanelViewModel.ProgressBars[0].Value += 10;
        }
    }
}