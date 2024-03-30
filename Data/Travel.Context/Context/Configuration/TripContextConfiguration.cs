namespace Travel.Context;

using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;

public static class TripContextConfiguration
{
    public static void ConfigureTrips(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>().ToTable("trips");
        modelBuilder.Entity<Trip>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Trip>().Property(x => x.Title).HasMaxLength(100);
        
        modelBuilder.Entity<Trip>()
             .HasOne(t => t.Creator)
             .WithMany(u => u.CreatedTrips)
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();
        modelBuilder.Entity<Trip>()
             .HasOne(t => t.Creator)
             .WithMany(u => u.CreatedTrips)
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

        modelBuilder.Entity<Trip>().HasMany(x => x.Participants).WithMany(x => x.ParticipatedTrips).UsingEntity(t => t.ToTable("user_trips"));


    }
}
