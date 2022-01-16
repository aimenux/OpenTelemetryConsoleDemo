using System.Diagnostics;
using App.Helpers;
using Microsoft.Extensions.Logging;

namespace App;

public class PlayerTwoService
{
    private readonly ILogger<PlayerTwoService> _logger;

    public PlayerTwoService(ILogger<PlayerTwoService> logger)
    {
        _logger = logger;
    }

    public async Task PongAsync()
    {
        using var activity = OpenTelemetrySource.Instance.StartActivity($"Activity.{nameof(PongAsync)}")!;
        activity.SetTag("player", "two");
        _logger.LogInformation("Pong");
        await Task.Delay(1000);
    }
}