using App.Extensions;
using App.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var _ = OpenTelemetryBootstrapper.CreateOpenTelemetryTracer();
        using var host = CreateHostBuilder(args).Build();
        var service = host.Services.GetRequiredService<GameService>();
        await service.RunAsync();

        Console.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddOpenTelemetry(options =>
                {
                    options
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenTelemetryLogger"))
                        .AddConsoleExporter();
                });
            })
            .ConfigureServices((_, services) =>
            {
                services.AddTransient<GameService>();
                services.AddTransient<PlayerOneService>();
                services.AddTransient<PlayerTwoService>();
            });
}