using System.Diagnostics;
using Microsoft.Extensions.Logging;
using static App.Extensions.OpenTelemetryExtensions;

namespace App;

public class PlayerOneService
{
    private readonly ILogger<PlayerOneService> _logger;

    private static readonly ActivitySource ActivitySource = CreateActivitySource<PlayerOneService>();

    public PlayerOneService(ILogger<PlayerOneService> logger)
    {
        _logger = logger;
    }

    public async Task PingAsync()
    {
        using var activity = ActivitySource.StartActivity($"Activity.{nameof(PingAsync)}")!;
        activity.AddEvent(new ActivityEvent("PlayerOne is playing"));
        activity.SetTag("player", "one");
        _logger.LogInformation("Ping");
        await Task.Delay(1000);
    }
}