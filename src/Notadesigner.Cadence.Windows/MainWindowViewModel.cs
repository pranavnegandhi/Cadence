using Notadesigner.Cadence.Windows.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notadesigner.Cadence.Windows;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title { get; init; } = GuiRunnerResources.ProductName;

    public double TimeElapsed { get; set; } = 0.5;

    private int _focusCounter;

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

    public TimeSpan ElapsedDuration { get; set; } = TimeSpan.FromMinutes(32);

    public TimeSpan TotalDuration { get; set; } = TimeSpan.FromMinutes(30);

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime FinishedAt { get; set; } = DateTime.UtcNow.AddMinutes(32);

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName is null)
        {
            return;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}