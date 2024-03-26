namespace Travel.Api;
using Travel.Services.Settings;
using Travel.Services.Logger;
using Travel.Context.Seeder;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices (this IServiceCollection service, IConfiguration configuration = null)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddAppLogger()
            .AddDbSeeder();
           // .AddUserAccountService();
        return service;
    }
}
