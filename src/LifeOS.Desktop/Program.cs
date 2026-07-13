    using System;
    using Avalonia;
    using LifeOS.Application;
    using LifeOS.Infrastructure;
    using LifeOS.Persistence;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    namespace LifeOS.Desktop;

    internal static class Program
    {
        // Avalonia requires a synchronous, attribute-free Main.
        [STAThread]
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Resolved once and passed into App.axaml.cs — this is the ONLY
            // static access point in the whole codebase, and it exists solely
            // because Avalonia's lifecycle predates the host being built.
            App.Services = host.Services;

            var logger = host.Services.GetRequiredService<Serilog.ILogger>();

            try
            {
                logger.Information("LifeOS starting up");
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "LifeOS terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                })
                .UseSerilog((context, services, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Debug()
                        .WriteTo.File(
                            path: "logs/lifeos-.log",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 14);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplication();
                    services.AddInfrastructure();
                    services.AddPersistence(context.Configuration);
                    services.AddDesktopShell();
                    services.AddHostedService<Services.EventReminderService>();
                });

        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
