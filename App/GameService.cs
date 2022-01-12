using System.Diagnostics;
using static App.Extensions.OpenTelemetryExtensions;

namespace App;

public class GameService
{
    private readonly PlayerOneService _playerOneService;
    private readonly PlayerTwoService _playerTwoService;

    private static readonly ActivitySource ActivitySource = CreateActivitySource<PlayerOneService>();

    public GameService(PlayerOneService playerOneService, PlayerTwoService playerTwoService)
    {
        _playerOneService = playerOneService;
        _playerTwoService = playerTwoService;
    }

    public async Task RunAsync()
    {
        using var activity = ActivitySource.StartActivity($"Activity.{nameof(RunAsync)}")!;

        activity.SetTag("GameStartDate", DateTime.UtcNow);
        
        await _playerOneService.PingAsync();
        await _playerTwoService.PongAsync();

        activity.SetTag("GameEndDate", DateTime.UtcNow);
    }
}