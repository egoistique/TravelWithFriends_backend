namespace Travel.Services.Activities;

using Microsoft.Extensions.DependencyInjection;
using Travel.Services.Categories;

public static class Bootstrapper
{
    public static IServiceCollection AddCategoryService(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICategoryService, CategoryService>();
    }
}
