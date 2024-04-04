using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Event_Registration;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

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

        // Individual's ExtraInformation max length
        modelBuilder.Entity<Individual>()
            .Property(i => i.ExtraInformation)
            .HasMaxLength(1000);

        // Business's ExtraInformation max length
        modelBuilder.Entity<Business>()
            .Property(b => b.ExtraInformation)
            .HasMaxLength(5000);

        // Event's ExtraInformation max length
        modelBuilder.Entity<Event>()
            .Property(e => e.ExtraInformation)
            .HasMaxLength(1000);
    }
}

public class Event
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required DateTime Time { get; set; }

    public required string Location { get; set; }
    public required string ExtraInformation { get; set; }

    public ICollection<Guest>? Guests { get; set; }
}

public abstract class Guest
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required Event Event { get; set; }
    public required PaymentType PaymentType { get; set; }
    public required string ExtraInformation { get; set; }
}

public class Individual : Guest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PersonalIdCode { get; set; }
}

public class Business : Guest
{
    public required string LegalName { get; set; }
    public required string RegistrationCode { get; set; }
    public int NumberOfAttendees { get; set; }
}

public enum PaymentType
{
    Cash,
    BankTransfer
}