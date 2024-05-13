namespace Travel.Api;
using Travel.Services.Settings;
using Travel.Services.Logger;
using Travel.Context.Seeder;
using Travel.Api.Settings;
using Travel.Services.Trips;
using Travel.Services.Activities;
using Travel.Services.UserAccount;
using Travel.Services.Statistics;
using Travel.Services.Categories;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices (this IServiceCollection service, IConfiguration configuration = null)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddIdentitySettings()
            .AddAppLogger()
            .AddDbSeeder()
            .AddApiSpecialSettings()
            .AddTripService()
            .AddActivityService()
            .AddStatisticsService()
            .AddUserAccountService()
            .AddCategoryService()
            .AddHttpClient();
        return service;
    }
}
