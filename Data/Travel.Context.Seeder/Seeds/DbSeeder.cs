﻿namespace Travel.Context.Seeder;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Travel.Services.UserAccount;

using System;

public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    }

    private static MainDbContext DbContext(IServiceProvider serviceProvider)
    {
        return ServiceScope(serviceProvider)
            .ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();
    }

    public static void Execute(IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
            {
                await AddAdministrator(serviceProvider);
                await AddDemoData(serviceProvider);
                
            })
            .GetAwaiter()
            .GetResult();
    }

    private static async Task AddDemoData(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
            return;

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddDemoData ?? false))
            return;

        await using var context = DbContext(serviceProvider);

        if (await context.Trips.AnyAsync())
            return;

        await context.Trips.AddRangeAsync(new DemoHelper().GetTrips);

        await context.SaveChangesAsync();
    }

    private static async Task AddAdministrator(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
            return;

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddAdministrator ?? false))
            return;

        var userAccountService = scope.ServiceProvider.GetService<IUserAccountService>();

        var adminEmail = "admin@travelwf.com";
        var adminExists = await userAccountService.Exists(adminEmail);
            
        if (adminExists)
            return;

        await userAccountService.Create(new RegisterUserAccountModel()
        {
            Name = settings.Init.Administrator.Name,
            Email = settings.Init.Administrator.Email,
            Password = settings.Init.Administrator.Password,
        });
    }

}