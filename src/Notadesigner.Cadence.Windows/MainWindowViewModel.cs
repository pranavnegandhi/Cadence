using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Cadence.Windows.Commands;
using Notadesigner.Cadence.Windows.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Notadesigner.Cadence.Windows;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _focusCounter;

    private TimeSpan _elapsedDuration;

    private TimeSpan _totalDuration;

    private DateTime _startedAt;

    private DateTime _finishedAt;

    public string Title { get; init; } = GuiRunnerResources.ProductName;

    public ICommand StartCommand { get; private set; }

    public MainWindowViewModel(IProducer<UIEvent> uiEventProducer)
    {
        StartCommand = new StartCommand(uiEventProducer);
    }

    public int FocusCounter
    {
        get => _focusCounter;

        set
        {
            if (_focusCounter == value)
            {
                return;
            }

            _focusCounter = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan ElapsedDuration
    {
        get => _elapsedDuration;

        set
        {
            if (_elapsedDuration == value)
            {
                return;
            }

            _elapsedDuration = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimeElapsed));
        }
    }

    public double TimeElapsed
    {
        get
        {
            var ratio = 100.0 * _elapsedDuration / _totalDuration;
            if (double.IsNaN(ratio))
            {
                ratio = 0.0;
            }

            return ratio;
        }
    }

    public TimeSpan TotalDuration
    {
        get => _totalDuration;

        set
        {
            if (_totalDuration == value)
            {
                return;
            }

            _totalDuration = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimeElapsed));
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

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName is null)
        {
            return;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}