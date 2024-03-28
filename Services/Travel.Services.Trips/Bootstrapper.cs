namespace Travel.Services.Trips;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddTripService(this IServiceCollection services)
    {
        return services
            .AddSingleton<ITripService, TripService>();            
    }
}