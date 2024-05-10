using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Travel.Common.Exceptions;
using Travel.Common.Validator;
using Travel.Common.Limits;
using Travel.Context;
using Travel.Context.Entities;

namespace Travel.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly IDbContextFactory<MainDbContext> dbContextFactory;
    private readonly IMapper mapper;

    public CategoryService(IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper

        )
    {
        this.dbContextFactory = dbContextFactory;
        this.mapper = mapper;

    }

    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var categories = await context.Categories
            .ToListAsync();

        var result = mapper.Map<IEnumerable<CategoryModel>>(categories);

        return result;
    }

    public async Task<CategoryModel> GetById(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var category = await context.Categories
            .FirstOrDefaultAsync(x => x.Uid == id);

        var result = mapper.Map<CategoryModel>(category);

        return result;
    }

    public async Task Delete(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var category = await context.Categories.Where(x => x.Uid == id).FirstOrDefaultAsync();

        if (category == null)
            throw new ProcessException($"category (ID = {id}) not found.");

        context.Categories.Remove(category);

        await context.SaveChangesAsync();
    }
}

