using System.Diagnostics;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace App.Extensions;

public static class OpenTelemetryExtensions
{
    public static ActivitySource CreateActivitySource<T>() => new($"ActivitySource.{typeof(T).Name}", "1.0.0");

    public static OpenTelemetryLoggerOptions ConfigureOpenTelemetry(this OpenTelemetryLoggerOptions options, string name)
    {
        Activity.ForceDefaultIdFormat = true;
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        ActivitySource.AddActivityListener(GetActivityListener());
        var builder = ResourceBuilder.CreateDefault().AddService(name);
        return options.SetResourceBuilder(builder);
    }

    private static ActivityListener GetActivityListener()
    {
        return new ActivityListener
        {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity => ConsoleColor.Green.WriteLine($"[BEGIN] {activity.ParentId}:{activity.Id}\n"),
            ActivityStopped = activity => ConsoleColor.Green.WriteLine($"[END] {activity.ParentId}:{activity.Id}\n")
        };
    }
}