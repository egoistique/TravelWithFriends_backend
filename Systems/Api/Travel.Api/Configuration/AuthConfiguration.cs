﻿namespace Travel.Api.Configuration;

using Travel.Common.Security;
using Travel.Context;
using Travel.Context.Entities;
using Travel.Services.Settings;
//using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuth(this IServiceCollection services /*, IdentitySettings settings */)
    {
        IdentityModelEventSource.ShowPII = true;

        services
            .AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders();




        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppScopes.TripsRead, policy => policy.RequireClaim("scope", AppScopes.TripsRead));
            options.AddPolicy(AppScopes.TripsWrite, policy => policy.RequireClaim("scope", AppScopes.TripsWrite));
        });

        return services;
    }

    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}