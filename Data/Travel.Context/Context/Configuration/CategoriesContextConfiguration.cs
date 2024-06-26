﻿namespace Travel.Context;

using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;

public static class CategoriesContextConfiguration
{
    public static void ConfigureCategories(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Category>().Property(x => x.Title).HasMaxLength(100);
        modelBuilder.Entity<Category>().HasIndex(x => x.Title).IsUnique();
    }
}