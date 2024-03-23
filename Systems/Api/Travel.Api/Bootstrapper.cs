namespace Travel.Api;
using Travel.Services.Settings;
using Travel.Services.Logger;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices (this IServiceCollection service, IConfiguration configuration = null)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddAppLogger();

        return service;
    }
}
