namespace Travel.Context;

using Travel.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class MainDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Activiti> Activities { get; set; }
    public DbSet<Category> Categories { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureTrips();
        modelBuilder.ConfigureActivities();
        modelBuilder.ConfigureCategories();
        modelBuilder.ConfigureUsers();
    }
}
