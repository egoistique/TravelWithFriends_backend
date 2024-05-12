namespace Travel.Services.Statistics;

using Microsoft.Extensions.DependencyInjection;
public static class Bootstrapper
{
    public static IServiceCollection AddStatisticsService(this IServiceCollection services)
    {
        return services
            .AddSingleton<IStatisticsService, StatisticsService>();
    }
}
