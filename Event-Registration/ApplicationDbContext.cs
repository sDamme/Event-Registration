using Event_Registration.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Registration;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Business> Businesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Guest>()
            .HasDiscriminator<int>("GuestType")
            .HasValue<Individual>(1)
            .HasValue<Business>(2);

        modelBuilder.Entity<Individual>()
            .Property(i => i.ExtraInformation)
            .HasMaxLength(1500);

        modelBuilder.Entity<Business>()
            .Property(b => b.ExtraInformation)
            .HasMaxLength(5000);

        modelBuilder.Entity<Event>()
            .Property(e => e.ExtraInformation)
            .HasMaxLength(1000);
    }
}