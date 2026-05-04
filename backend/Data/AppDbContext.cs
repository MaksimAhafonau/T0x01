using Microsoft.EntityFrameworkCore;
using SpaceDC.Models;
using SpaceDC.Models.Enums;

namespace SpaceDC.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ResourceSchedule> ResourceSchedules => Set<ResourceSchedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).HasMaxLength(320).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.Role).HasConversion<int>();
        });

        modelBuilder.Entity<Resource>(e =>
        {
            e.ToTable("resources");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Type).HasConversion<int>();
            e.Property(x => x.Status).HasConversion<int>();
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.ToTable("bookings");
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).HasConversion<int>();
            e.HasIndex(x => new { x.ResourceId, x.StartTime, x.EndTime });
            e.HasOne(x => x.User)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Resource)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ResourceSchedule>(e =>
        {
            e.ToTable("resource_schedules");
            e.HasKey(x => x.Id);
            e.Property(x => x.DayOfWeek).HasConversion<int>();
            e.HasIndex(x => new { x.ResourceId, x.DayOfWeek });
            e.HasOne(x => x.Resource)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
