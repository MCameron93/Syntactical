using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SyntacticalPetApp
{
    public class DogCommandsViewModel : INotifyPropertyChanged
    {
        public EventHandler BugsFixedEnabled;
        public EventHandler DogFed;

        public EventHandler PartyModeEntered;

        private bool bugsFixed;
        private RelayCommand fixAllBugsCommand;

        private RelayCommand fixAllBugsCommand1;
        private bool partyModeEnabled;

        public DogCommandsViewModel()
        {
            FeedCommand = new RelayCommand(_ => Feed(), _ => !PartyModeEnabled);
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
                    fixAllBugsCommand1 = new RelayCommand(FixAllBugs, _ => !BugsFixed);
                }

                return fixAllBugsCommand1;
            }
        }

        public ICommand PartyCommand { get; }

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

        public ProgressPanelViewModel StatusViewModel { get; set; }

        private bool BugsFixed
        {
            get => bugsFixed;
            set
            {
                MessageBox.Show(messageBoxText: "You thought it would be that easy?", caption: "Oh no!",
                    MessageBoxButton.OK, MessageBoxImage.Question);

                bugsFixed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BugsFixed)));
                BugsFixedEnabled?.Invoke(this, EventArgs.Empty);
            }
        }

        private double Happiness => StatusViewModel.Bars[1].Value;

        private void IncreaseHappiness()
        {
            StatusViewModel.Bars[1].Value += 20;
        }

        private void DecreaseHunger()
        {
            StatusViewModel.Bars[2].Value -= 10;
        }

        private void Feed()
        {
            // Note: It's bad form to overfeed your dogs.
            DecreaseHunger();
            //IncreaseHappiness();
            //DogFed?.Invoke(this, EventArgs.Empty);
        }

        private void EnterPartyMode()
        {
            if (Happiness < 100)
            {
                MessageBox.Show(messageBoxText: "Only happy dogs can party.", caption: "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            MessageBox.Show(messageBoxText: "Party mode has not been implemented yet.", caption: "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            
            //PartyModeEnabled = true;
        }

        private void FixAllBugs(object commandParameter)
        {
            MessageBox.Show(messageBoxText: "You forgot to implement the bug fix logic!", caption: "Woops",
                MessageBoxButton.OK, MessageBoxImage.Error);
            
            //BugsFixed = true;
        }
    }
}