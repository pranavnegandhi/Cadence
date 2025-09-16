using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Cadence.Windows.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Notadesigner.Cadence.Windows;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _focusCounter;

    private TimeSpan _elapsedDuration;

    private TimeSpan _totalDuration;

    private DateTime _startedAt;

    private DateTime _finishedAt;

    private TimerState _timerState;

    private readonly MainWindow _mainWindow;

    public App(IProducer<UIEvent> uiEventProducer,
            [FromKeyedServices("guiTransition")] IEventHandler<TransitionEvent> transitionHandler,
            [FromKeyedServices("guiTimer")] IEventHandler<TimerEvent> timerHandler,
            MainWindow mainWindow)
    {
        InitializeComponent();

        var handler = (GuiTransitionEventHandler)transitionHandler;
        handler.Abandoned += AbandonedEventHandler;
        handler.Begin += BeginEventHandler;
        handler.End += EndEventHandler;
        handler.Finished += FinishedEventHandler;
        handler.FocusedEntry += FocusedEventHandler;
        handler.Interrupted += InterruptedEventHandler;
        handler.Refreshed += RefreshedEventHandler;
        handler.Relaxed += RelaxedEventHandler;
        handler.Stopped += StoppedEventHandler;

        timerHandler.EventReceived += TimerEventReceivedHandler;

        StartCommand = new StartCommand(uiEventProducer);

        _mainWindow = mainWindow;
        _mainWindow.DataContext = this;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _mainWindow.Show();
        base.OnStartup(e);
    }

    public ICommand StartCommand { get; private set; }

    public int FocusCounter
    {
        get => _focusCounter;

        set
        {
            if (value != _focusCounter)
            {
                _focusCounter = value;

                OnPropertyChanged();
            }
        }
    }

    public TimeSpan ElapsedDuration
    {
        get => _elapsedDuration;

        set
        {
            if (value != _elapsedDuration)
            {
                _elapsedDuration = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeSpan TotalDuration
    {
        get => _totalDuration;

        set
        {
            if (value != _totalDuration)
            {
                _totalDuration = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime StartedAt
    {
        get => _startedAt;

        set
        {
            if (_startedAt == value)
            {
                return;
            }

            _startedAt = value;
            OnPropertyChanged();
        }
    }

    public DateTime FinishedAt
    {
        get => _finishedAt;

        set
        {
            if (_finishedAt == value)
            {
                return;
            }

            _finishedAt = value;
            OnPropertyChanged();
        }
    }

    public TimerState TimerState
    {
        get => _timerState;

        set
        {
            if (value != _timerState)
            {
                _timerState = value;
                OnPropertyChanged();
            }
        }
    }

    private void AbandonedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Abandoned;
        FocusCounter = e;
    }

    private void BeginEventHandler(object? _, int e)
    {
        TimerState = TimerState.Begin;
        FocusCounter = e;
        ElapsedDuration = TimeSpan.Zero;
        StartedAt = DateTime.Now;
    }

    private void FocusedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Focused;
        FocusCounter = e;
    }

    private void EndEventHandler(object? _, int e)
    {
        TimerState = TimerState.End;
        FocusCounter = e;
    }

    private void FinishedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Finished;
        FocusCounter = e;
    }

    private void InterruptedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Interrupted;
        FocusCounter = e;
    }

    private void RefreshedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Refreshed;
        FocusCounter = e;
    }

    private void RelaxedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Relaxed;
        FocusCounter = e;
    }

    private void StoppedEventHandler(object? _, int e)
    {
        TimerState = TimerState.Stopped;
        FocusCounter = e;
    }

    private void TimerEventReceivedHandler(object? sender, TimerEvent @event)
    {
        ElapsedDuration = @event.Elapsed;
        TotalDuration = @event.TotalDuration;
        FinishedAt = StartedAt.Add(TotalDuration);
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}