using Microsoft.Extensions.DependencyInjection;
using Portfolio.Domain.Interfaces.Repositories;
using Portfolio.Infrastructure.Repositories;

namespace Portfolio.Infrastructure.Extensions;

public static class InfraServiceConfigurations
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ImageOfTheDayRepository, ImageOfTheDayRepository>();
        services.AddScoped<IRequestLogRepository, RequestLogRepository>();
    }
}