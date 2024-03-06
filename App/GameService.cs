using System.Diagnostics;
using App.Helpers;
using Microsoft.Extensions.Logging;

namespace App;

public class GameService
{
    private readonly PlayerOneService _playerOneService;
    private readonly PlayerTwoService _playerTwoService;
    private readonly ILogger<GameService> _logger;

    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    public string GameName { get; } = $"{Guid.NewGuid():N}";

    public GameService(PlayerOneService playerOneService, PlayerTwoService playerTwoService, ILogger<GameService> logger)
    {
        _playerOneService = playerOneService;
        _playerTwoService = playerTwoService;
        _logger = logger;
    }

    public async Task RunAsync(int rounds = 5)
    {
        if (IsRunningInsideDocker())
        {
            _logger.LogTrace("Waiting for zipkin container to be ready ..");
            _logger.LogInformation("Game {GameName} will start very soon ..", GameName);
            await Task.Delay(Delay);
        }

        for (var round = 1; round <= rounds; round++)
        {
            var activityName = $"Activity.{nameof(RunAsync)}.Round.{round}";
            using var activity = OpenTelemetrySource.Instance.StartActivity(activityName)!;
            activity.AddEvent(new ActivityEvent($"Game {GameName} is starting"));
            await _playerOneService.PingAsync();
            await _playerTwoService.PongAsync();
            activity.AddEvent(new ActivityEvent($"Game {GameName} is stopping"));
            await Task.Delay(Delay);
        }
    }

    private static bool IsRunningInsideDocker() => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
}