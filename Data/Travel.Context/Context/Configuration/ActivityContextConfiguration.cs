namespace Travel.Context;

using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;

public static class ActivityContextConfiguration
{
    public static void ConfigureActivities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activiti>().ToTable("activities");
        modelBuilder.Entity<Activiti>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Activiti>().Property(x => x.Title).HasMaxLength(250);
        modelBuilder.Entity<Activiti>()
            .HasOne(x => x.Day)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.DayId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Activiti>()
            .HasOne(x => x.Category)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Activiti>()
            .HasOne(x => x.Payer)
            .WithMany(x => x.PayedActivities)
            .HasForeignKey(x => x.PayerId);

        modelBuilder.Entity<Activiti>().HasMany(x => x.Participants).WithMany(x => x.ParticipatedActivities).UsingEntity(t => t.ToTable("user_activities"));

    }
}
