using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Portfolio.Infrastructure.Helpers;
using Serilog;

namespace Portfolio.Infrastructure.Extensions;

public static class OpenTelemetryExtensions
{
    public static void InitOpenTelemetry(this WebApplicationBuilder builder)
    {
        Action<ResourceBuilder> appResourceBuilder =
            resource => resource
                .AddContainerDetector()
                .AddHostDetector()
                .AddOperatingSystemDetector()
                .AddService(AssemblyHelper.GetStartupProjectsName() + " " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

        var otelEndpoint = Environment.GetEnvironmentVariable("OTEL_COLLECTOR_ENDPOINT");
        if (string.IsNullOrEmpty(otelEndpoint))
        {
            Log.Error("Otel endpoint not set at environment variable! Please set environment variable OTEL_COLLECTOR_ENDPOINT");
            return;
        }
        
        Log.Information("Starting Open Telemetry with endpoint: {OtelEndpoint}", otelEndpoint);
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(appResourceBuilder)
            .WithTracing(tracerBuilder => tracerBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddGrpcCoreInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(otelEndpoint);
                        o.Protocol = OtlpExportProtocol.Grpc;
                    })
                // .AddConsoleExporter()
            ) 
            .WithMetrics(meterBuilder => meterBuilder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(otelEndpoint);
                        o.Protocol = OtlpExportProtocol.Grpc;
                    })
                // .AddConsoleExporter()
            );
    }
}