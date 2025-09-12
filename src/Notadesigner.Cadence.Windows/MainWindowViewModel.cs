using Notadesigner.Cadence.Windows.Properties;
using System.ComponentModel;

namespace Notadesigner.Cadence.Windows;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title { get; init; } = GuiRunnerResources.ProductName;

    public double TimeElapsed { get; set; } = 0.5;

    public int FocusCounter { get; set; } = 3;

    public TimeSpan ElapsedDuration { get; set; } = TimeSpan.FromMinutes(32);

    public TimeSpan TotalDuration { get; set; } = TimeSpan.FromMinutes(30);

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime FinishedAt { get; set; } = DateTime.UtcNow.AddMinutes(32);
}