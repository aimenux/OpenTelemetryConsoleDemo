using System.Diagnostics;
using App.Helpers;
using Microsoft.Extensions.Logging;

namespace App;

public class PlayerOneService
{
    private readonly ILogger<PlayerOneService> _logger;

    public PlayerOneService(ILogger<PlayerOneService> logger)
    {
        _logger = logger;
    }

    public async Task PingAsync()
    {
        using var activity = OpenTelemetrySource.Instance.StartActivity($"Activity.{nameof(PingAsync)}")!;
        activity.SetTag("player", "one");
        _logger.LogInformation("Ping");
        await Task.Delay(1000);
    }
}