using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Portfolio.DataAccess.Helpers;

public static class StartupHelper
{
    // todo: send this config to chatgpt for asking what minimumlevel does. And after, add this for prod env: https://github.com/b00ted/serilog-sinks-postgresql
    public static void InitLogsWithSerilog(this WebApplicationBuilder builder)
    {
        using var log = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
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