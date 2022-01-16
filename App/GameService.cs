using System.Diagnostics;
using App.Helpers;

namespace App;

public class GameService
{
    private readonly PlayerOneService _playerOneService;
    private readonly PlayerTwoService _playerTwoService;

    public string GameName { get; } = $"{Guid.NewGuid():P}";

    public GameService(PlayerOneService playerOneService, PlayerTwoService playerTwoService)
    {
        _playerOneService = playerOneService;
        _playerTwoService = playerTwoService;
    }

    public async Task RunAsync()
    {
        using var activity = OpenTelemetrySource.Instance.StartActivity($"Activity.{nameof(RunAsync)}")!;
        activity.AddEvent(new ActivityEvent($"Game {GameName} is starting"));
        await _playerOneService.PingAsync();
        await _playerTwoService.PongAsync();
        activity.AddEvent(new ActivityEvent($"Game {GameName} is stopping"));
    }
}