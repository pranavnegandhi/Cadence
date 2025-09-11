using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;

namespace Notadesigner.Cadence.Windows;

public class GuiTimerEventHandler(App app) : IEventHandler<TimerEvent>
{
    public event EventHandler<TimerEvent>? EventReceived;

    async ValueTask IEventHandler<TimerEvent>.HandleAsync(TimerEvent @event, CancellationToken cancellationToken)
    {
        await app.Dispatcher.InvokeAsync(() => EventReceived?.Invoke(this, @event));
    }
}