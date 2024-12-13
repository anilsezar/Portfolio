using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;

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
                    o.Endpoint = new Uri("http://otel-collector.default.svc.cluster.local:4317");
                    o.Protocol = OtlpExportProtocol.Grpc;
                })
                // .AddConsoleExporter()
                .AddEntityFrameworkCoreInstrumentation()
            ) 
            .WithMetrics(meterBuilder => meterBuilder
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("http://otel-collector.default.svc.cluster.local:4317");
                    o.Protocol = OtlpExportProtocol.Grpc;
                })
                // .AddConsoleExporter()
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

    private static string GetConnectionStringForPostgres(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
            Env.Load("../.env");

        var host = Environment.GetEnvironmentVariable("SQL_DB_HOST");
        var port = Environment.GetEnvironmentVariable("SQL_DB_PORT");
        var userName = Environment.GetEnvironmentVariable("SQL_DB_USER");
        var userPass = Environment.GetEnvironmentVariable("SQL_DB_PASSWORD");
        var dbName = Environment.GetEnvironmentVariable("SQL_DB_NAME");

        if (string.IsNullOrEmpty(host) || 
            string.IsNullOrEmpty(port) ||
            string.IsNullOrEmpty(userName) ||
            string.IsNullOrEmpty(userPass) ||
            string.IsNullOrEmpty(dbName)
            )
        {
            Log.Fatal("One or more db ConnectionString value(s) is not set.");
            throw new InvalidOperationException();
        }

        // todo: Stop using default public schema
        return $"Host={host};Port={port};Username={userName};Password={userPass};Database={dbName};";
    }
    
    public static void DbInitWithPostgres(this WebApplicationBuilder builder)
    {
        var connectionString = builder.GetConnectionStringForPostgres();

        var appName = AssemblyHelper.GetStartupProjectsName();
        builder.Services.AddDbContext<WebAppDbContext>(options =>
        {
            options.UseNpgsql(connectionString + $";Application Name= {appName}",
                npgsqlOptionsAction: sqlOptions =>
                {
                    // sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, dbSchemaName);
                    // sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                }).UseSnakeCaseNamingConvention();

            if (!builder.Environment.IsDevelopment()) return;
            
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }
}