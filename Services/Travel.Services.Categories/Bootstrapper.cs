namespace Travel.Services.Categories;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddCategoryService(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICategoryService, CategoryService>();
    }
}
