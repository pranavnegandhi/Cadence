using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;

namespace Notadesigner.Cadence.Windows;

public class GuiTimerEventHandler(IWpfContext context) : IEventHandler<TimerEvent>
{
    public event EventHandler<TimerEvent>? EventReceived;

    async ValueTask IEventHandler<TimerEvent>.HandleAsync(TimerEvent @event, CancellationToken cancellationToken)
    {
        await context.Dispatcher.InvokeAsync(() => EventReceived?.Invoke(this, @event));
    }
}