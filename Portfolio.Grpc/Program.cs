using DotNetEnv;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Portfolio.Infrastructure.Constants;
using Portfolio.Grpc.Services;
using Portfolio.Grpc.Services.VisitorInsightsServices;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    Env.Load("../.env");

EnvironmentExtensions.VerifyEnvironmentValuesAreSet([
    EnvironmentVariableNames.SqlDb_Host, 
    EnvironmentVariableNames.SqlDb_Port, 
    EnvironmentVariableNames.SqlDb_User, 
    EnvironmentVariableNames.SqlDb_Password, 
    EnvironmentVariableNames.SqlDb_Name,
    EnvironmentVariableNames.OpenTelemetry_CollectorEndpoint,
    EnvironmentVariableNames.DevOrProd
]);

builder.InitLogsWithSerilog();
builder.InitOpenTelemetry();
builder.InitDbWithPostgres();

const string readinessDbCheckName = "databaseConnectionActive";
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<PortfolioDbContext>(failureStatus: HealthStatus.Degraded, name: readinessDbCheckName);

builder.Services.AddGrpc();
builder.Services.ConfigureRepositories();

var app = builder.Build();

// todo: Maybe do a grpc endpoint check? Or maybe read this healthcheck stuff again. Seems like I'm missing something. 
app.MapHealthChecks(DefaultValues.HealthCheck_Liveness, new HealthCheckOptions
{
    Predicate = _ => false, // Always return healthy for liveness
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK, // Liveness doesn't degrade
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecks(DefaultValues.HealthCheck_Readiness, new HealthCheckOptions
{
    Predicate = check => check.Name == readinessDbCheckName,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
app.MapGrpcService<GetBackgroundImageService>();
app.MapGrpcService<VisitorInsightsService>();


app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

try
{
    Log.Information("✅ App Starting");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ The app failed to start");
}
finally
{
    Log.CloseAndFlush();
}