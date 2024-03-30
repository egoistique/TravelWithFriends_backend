namespace Travel.Context;

using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
public static class TripDayContextConfiguration
{
    public static void ConfigureDays(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripDay>().ToTable("days");
        modelBuilder.Entity<TripDay>().Property(x => x.Number).IsRequired();
        modelBuilder.Entity<TripDay>()
            .HasOne(x => x.Trip)
            .WithMany(x => x.Days)
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
