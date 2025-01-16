using System.Windows.Input;

namespace EngineTool
{
    public class DelegateCommand : ICommand
    {
        public readonly Action<object?> _execute;
        public readonly Func<object?, bool> _canExecute;

        public event EventHandler? CanExecuteChanged;


        public DelegateCommand(Func<object?, bool> canExecute, Action<object?> execute)
            => (_canExecute, _execute) = (canExecute, execute);

        public DelegateCommand(Action<object?> execute) : this(_ => true, execute) { }


        public bool CanExecute(object? parameter) 
            => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) 
            => _execute?.Invoke(parameter);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
