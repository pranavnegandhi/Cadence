using Notadesigner.Cadence.Windows.Properties;
using System.ComponentModel;

namespace Notadesigner.Cadence.Windows;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title { get; init; } = GuiRunnerResources.ProductName;
}