using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

namespace Portfolio.DataAccess.Helpers;

public static class StartupHelper
{
    public static void InitOpenTelemetry(this WebApplicationBuilder builder)
    {
        Action<ResourceBuilder> appResourceBuilder =
            resource => resource
                .AddContainerDetector()
                .AddHostDetector()
                .AddService("Portfolio Website");
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(appResourceBuilder)
            .WithTracing(tracerBuilder => tracerBuilder
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("jaeger-service:4317");
                    o.Protocol = OtlpExportProtocol.Grpc;
                })
                .AddConsoleExporter()
                .AddEntityFrameworkCoreInstrumentation()
            ) 
            .WithMetrics(meterBuilder => meterBuilder
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("jaeger-service:4317");
                    o.Protocol = OtlpExportProtocol.Grpc;
                })
                .AddConsoleExporter()
            );
    }
    
    // todo: send this config to chatgpt for asking what minimumlevel does. And after, add this for prod env: https://github.com/b00ted/serilog-sinks-postgresql
    public static void InitLogsWithSerilog(this WebApplicationBuilder builder)
    {
        using var log = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            // .WriteTo.OpenTelemetry(
            //     endpoint: "http://127.0.0.1:4318/v1/logs",
            //     protocol: OtlpProtocol.Grpc
            //     )
            .CreateLogger();
        builder.Host.UseSerilog(log);
    }
    
    public static void DbInitWithPostgres(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();

        // todo: Stop using default public schema

        var appName = AssemblyHelper.GetStartupProjectsName();
        builder.Services.AddDbContext<WebAppDbContext>(options =>
        {
            options.UseNpgsql(connectionString + $";Application Name= {appName}",
                npgsqlOptionsAction: sqlOptions =>
                {
                    // sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, dbSchemaName);
                    // sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                }).UseSnakeCaseNamingConvention();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
    }
}