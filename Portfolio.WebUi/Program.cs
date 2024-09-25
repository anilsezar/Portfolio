using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Portfolio.DataAccess;
using Portfolio.DataAccess.Helpers;
using Portfolio.Domain.Helpers;
using Portfolio.WebUi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.InitLogsWithSerilog();

if (builder.Environment.IsDevelopment())
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
else
    builder.Services.AddRazorPages();

builder.Services.AddScoped<BackgroundImageFromBingService>();

builder.InitOpenTelemetry();

builder.DbInitWithPostgres();

const string readinessDbCheckName = "databaseConnectionActive";
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<WebAppDbContext>(failureStatus: HealthStatus.Degraded, name: readinessDbCheckName);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = _ => false, // Always return healthy for liveness
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK, // Liveness doesn't degrade
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecks("/readiness", new HealthCheckOptions
{
    Predicate = check => check.Name == readinessDbCheckName,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

try
{
    Log.Information("App Starting.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The app failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
