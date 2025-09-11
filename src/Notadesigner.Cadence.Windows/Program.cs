using Microsoft.EntityFrameworkCore;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Data;
using Notadesigner.Approximato.Messaging.ServiceRegistration;
using Notadesigner.Cadence.Windows.Properties;
using Serilog;
using System.Runtime.Versioning;

namespace Notadesigner.Cadence.Windows;

internal static class Program
{
    [STAThread]
    [SupportedOSPlatform("windows")]
    public static async Task Main(string[] args)
    {
        /// Initialize the application logging framework
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
        var configuration = configurationBuilder.Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            /// Attempt to initialize the service
            Log.Information("Starting Cadence");

            var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);
            builder.Host
                .ConfigureServices(ConfigureServicesDelegate)
                .UseSerilog();

            var app = builder.Build();
            await app.Services.StartConsumers();

            await app.RunAsync();
        }
        catch (ApplicationException exception)
        {
            Log.Fatal(exception, "Approximato was not launched correctly.");
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Error encountered while running Approximato.");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void ConfigureServicesDelegate(HostBuilderContext context, IServiceCollection collection)
    {
        var appSettings = GuiRunnerSettings.Default;
        StateHostSettings settingsFactory() => new(appSettings.MaximumRounds,
            appSettings.FocusDuration,
            appSettings.ShortBreakDuration,
            appSettings.LongBreakDuration,
            appSettings.LenientMode);

        collection.AddSingleton(settingsFactory)
            .AddTransient<MainWindowViewModel>();

        collection.CreateEvent<UIEvent>()
            .AddEventHandler<UIEvent, StateHost>()
            .CreateEvent<TransitionEvent>()
            .AddEventHandler<TransitionEvent, GuiTransitionEventHandler>("guiTransition")
            .AddEventHandler<TransitionEvent, DbTransitionEventHandler>()
            .CreateEvent<TimerEvent>()
            .AddEventHandler<TimerEvent, GuiTimerEventHandler>("guiTimer");

        collection.AddScoped<TransitionStorageService>()
            .AddDbContext<ApproximatoDbContext>(options =>
            {
                var connectionString = context.Configuration.GetConnectionString("PrimaryStorage");
                options.UseSqlite(connectionString);
            }, ServiceLifetime.Transient);
    }
}