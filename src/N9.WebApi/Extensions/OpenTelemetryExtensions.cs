using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace N9.WebApi.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
        });

        var otel = builder.Services.AddOpenTelemetry();

        otel.WithMetrics(metrics => { metrics.AddAspNetCoreInstrumentation(); });

        otel.WithTracing(trace =>
        {
            trace.AddAspNetCoreInstrumentation();
            trace.AddHttpClientInstrumentation();
        });

        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (otlpEndpoint != null) otel.UseOtlpExporter();

        var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (appInsightsConnectionString != null) otel.UseAzureMonitor();

        return builder;
    }
}