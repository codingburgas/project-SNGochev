using EventBookingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .Property(e => e.TicketPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Events)
            .WithOne(e => e.Category!)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Tickets)
            .WithOne(t => t.Event!)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Tickets)
            .WithOne(t => t.ApplicationUser!)
            .HasForeignKey(t => t.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
