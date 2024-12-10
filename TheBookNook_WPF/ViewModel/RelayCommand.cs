using System.Windows.Input;

namespace TheBookNook_WPF.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeCommand;
        private readonly Func<object,bool> _canExecuteCommand;

        public RelayCommand(Action<object> executeCommand, Func<object,bool> canExecuteCommand = null)
        {
            _executeCommand = executeCommand;
            _canExecuteCommand = canExecuteCommand;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecuteCommand == null ? true : _canExecuteCommand(parameter);
        }

        public void Execute(object? parameter)
        {
            _executeCommand(parameter);
        }
    }
}
