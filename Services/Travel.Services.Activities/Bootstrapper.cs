namespace Travel.Services.Activities;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddActivityService(this IServiceCollection services)
    {
        return services
            .AddSingleton<IActivityService, ActivityService>();
    }
}