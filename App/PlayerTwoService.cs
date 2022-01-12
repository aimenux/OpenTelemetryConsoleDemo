using System.Diagnostics;
using Microsoft.Extensions.Logging;
using static App.Extensions.OpenTelemetryExtensions;

namespace App;

public class PlayerTwoService
{
    private readonly ILogger<PlayerTwoService> _logger;

    private static readonly ActivitySource ActivitySource = CreateActivitySource<PlayerOneService>();

    public PlayerTwoService(ILogger<PlayerTwoService> logger)
    {
        _logger = logger;
    }

    public async Task PongAsync()
    {
        using var activity = ActivitySource.StartActivity($"Activity.{nameof(PongAsync)}")!;
        activity.AddEvent(new ActivityEvent("PlayerTwo is playing"));
        activity.SetTag("player", "two");
        _logger.LogInformation("Pong");
        await Task.Delay(1000);
    }
}