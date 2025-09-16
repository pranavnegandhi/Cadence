using System.Windows.Input;

namespace Notadesigner.Cadence.Windows.Commands;

public class ExitCommand(Action<object?> execute, Predicate<object?> canExecute) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;

        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => canExecute(parameter);

    public void Execute(object? parameter) => execute(parameter);
}