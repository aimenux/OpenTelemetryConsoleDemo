using System.Diagnostics;
using Microsoft.Extensions.Configuration;
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

    public static TracerProvider? CreateOpenTelemetryTracer(IConfiguration configuration)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService("OpenTelemetryTracer");

        var openTelemetryTracer = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource(OpenTelemetrySource.SourceName)
            .SetResourceBuilder(resourceBuilder)
            .AddConsoleExporter()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri(configuration.GetValue<string>("ZipKin"));
            })
            .Build();

        return openTelemetryTracer;
    }
}