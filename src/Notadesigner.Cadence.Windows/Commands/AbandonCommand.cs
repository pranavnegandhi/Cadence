using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using System.Windows.Input;

namespace Notadesigner.Cadence.Windows.Commands;

public class AbandonCommand(IProducer<UIEvent> uiEventProducer) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _ = Task.Run(async () =>
        {
            var payload = new UIEvent(TimerTrigger.Abandon);
            var @event = new Event<UIEvent>(payload);
            await uiEventProducer.PublishAsync(@event);
        });
    }
}