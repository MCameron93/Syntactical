using System;
using System.Windows.Input;

namespace Syntactical
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action executeAction;

        public RelayCommand(Action executeAction)
        {
            this.executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction.Invoke();
        }
    }
}