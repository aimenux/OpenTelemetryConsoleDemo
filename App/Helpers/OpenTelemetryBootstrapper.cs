using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace App.Helpers;

public static class OpenTelemetryBootstrapper
{
    static OpenTelemetryBootstrapper()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    }

    public static TracerProvider? CreateOpenTelemetryTracer()
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService("OpenTelemetryTracer");

        var openTelemetryTracer = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource(OpenTelemetrySource.SourceName)
            .SetResourceBuilder(resourceBuilder)
            .AddConsoleExporter()
            .Build();

        return openTelemetryTracer;
    }
}