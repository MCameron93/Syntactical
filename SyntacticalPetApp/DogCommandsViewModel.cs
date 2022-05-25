using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SyntacticalPetApp
{
    public class DogCommandsViewModel : INotifyPropertyChanged
    {
        public EventHandler BugsFixedEnabled;

        public EventHandler PartyModeEntered;

        private RelayCommand fixAllBugsCommand;

        private RelayCommand fixAllBugsCommand1;
        private bool partyModeEnabled;

        public DogCommandsViewModel()
        {
            FeedCommand = new RelayCommand(_ => Feed());
            PartyCommand = new RelayCommand(_ => EnterPartyMode(), _ => !PartyModeEnabled);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand FeedCommand { get; }

        public ICommand FixAllBugsCommand
        {
            get
            {
                if (fixAllBugsCommand1 == null)
                {
                    fixAllBugsCommand1 = new RelayCommand(FixAllBugs, _=> !BugsFixed);
                }

                return fixAllBugsCommand1;
            }
        }

        public ICommand PartyCommand { get; }
        public ProgressPanelViewModel ProgressPanelViewModel { get; set; }
        public bool PartyModeEnabled
        {
            get => partyModeEnabled;
            set
            {
                partyModeEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartyModeEnabled)));
                PartyModeEntered?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool bugsFixed;
        private bool BugsFixed
        {
            get => bugsFixed;
            set
            {
                bugsFixed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BugsFixed)));
                BugsFixedEnabled?.Invoke(this, EventArgs.Empty);
            }
        }

        private void EnterPartyMode()
        {
            MessageBox.Show(messageBoxText: "Party mode has not been implemented yet.", caption: "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            //PartyModeEnabled = true;
        }

        private void Feed()
        {
            // Note: It's bad form to overfeed your dogs.
            ProgressPanelViewModel.ProgressBars[1].Value += 10;
            ProgressPanelViewModel.ProgressBars[2].Value -= 10;
        }

        private void FixAllBugs(object commandParameter)
        {
            MessageBox.Show(messageBoxText: "You thought it would be that easy?", caption: "Idiot",
                MessageBoxButton.OK, MessageBoxImage.Error);

            //BugsFixed = true;
        }
    }
}