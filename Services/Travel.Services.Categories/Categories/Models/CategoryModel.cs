using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Context.Entities;
using Travel.Context;

namespace Travel.Services.Categories;

/// <summary>
/// Model representing a category.
/// </summary>
public class CategoryModel
{
    /// <summary>
    /// The ID of the category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the category.
    /// </summary>
    public string Title { get; set; }
}

public class CategoryModelProfile : Profile
{
    public CategoryModelProfile()
    {
        CreateMap<Category, CategoryModel>()
            .BeforeMap<CategoryModelActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())

            ;
    }
    public class CategoryModelActions : IMappingAction<Category, CategoryModel>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CategoryModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(Category source, CategoryModel destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var category = db.Categories
                .FirstOrDefault(x => x.Id == source.Id);


            destination.Id = category.Uid;

        }
    }
}

