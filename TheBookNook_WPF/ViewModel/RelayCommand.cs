using System.Windows.Input;

namespace TheBookNook_WPF.ViewModel
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _executeCommand;
        private readonly Predicate<object> _canExecuteCommand;

        public RelayCommand(Action<object> executeCommand)
        {
            _executeCommand = executeCommand;
            _canExecuteCommand = null;
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
